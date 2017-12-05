﻿using Albion_Direct;
using Merlin.Pathing;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using YinYang.CodeProject.Projects.SimplePathfinding.PathFinders.AStar;
using Stateless;

namespace Merlin.Profiles.Gatherer
{
    public sealed partial class Gatherer
    {
        enum HarvestState
        {
            Enter,
            Mounting,
            TravelToResource,
            TravelToMob,
            WalkToResource,
            WalkToMob,
            AttackMob,
            HarvestResource,
            HarvestMob,
            UnstickYourself
        }

        enum HarvestTrigger
        {
            StartHarvest,
            StartMounting,
            StartHarvestingResource,
            StartHarvestingMob,
            StartWalkingToResource,
            StartWalkingToMob,
            StartAttackingMob,
            StartTravelingToResource,
            StartTravelingToResourceWithLittleWait,
            StartTravelingToMob,
            StartUnstickingYourself
        }

        public const double MeleeAttackRange = 2.5;
        const float InteractRange = 3f; // Don't remount when you just killed a mob.

        static readonly TimeSpan _timeToUnstick = TimeSpan.FromSeconds(1.0);
        DateTime _unstickStartTime = DateTime.Now;

        const float _maxTravelWaitTime = 3f;
        DateTime _travelStartTime = DateTime.Now;

        ClusterPathingRequest _harvestPathingRequest;
        StateMachine<HarvestState, HarvestTrigger> _harvestState;

        void HarvestOnStart()
        {
            _harvestState = new StateMachine<HarvestState, HarvestTrigger>(HarvestState.Enter);
            _harvestState.Configure(HarvestState.Enter)
                .OnEntry(() => OnHarvestEnter())
                .PermitReentry(HarvestTrigger.StartHarvest)
                .Permit(HarvestTrigger.StartTravelingToResourceWithLittleWait, HarvestState.TravelToResource)
                .Permit(HarvestTrigger.StartWalkingToResource, HarvestState.WalkToResource)
                .Permit(HarvestTrigger.StartTravelingToMob, HarvestState.TravelToMob)
                .Permit(HarvestTrigger.StartWalkingToMob, HarvestState.WalkToMob)
                .Permit(HarvestTrigger.StartMounting, HarvestState.Mounting);

            _harvestState.Configure(HarvestState.Mounting)
                .OnEntry(() => OnMountingEnter())
                .Permit(HarvestTrigger.StartHarvest, HarvestState.Enter);

            // Resources
            _harvestState.Configure(HarvestState.TravelToResource)
                .OnEntry(() => OnTravelToResourceEnter())
                .OnEntryFrom(HarvestTrigger.StartTravelingToResourceWithLittleWait, () => OnTravelToResourceEnter(true))
                .Permit(HarvestTrigger.StartWalkingToResource, HarvestState.WalkToResource)
                .Permit(HarvestTrigger.StartUnstickingYourself, HarvestState.UnstickYourself)
                .Permit(HarvestTrigger.StartHarvest, HarvestState.Enter);

            _harvestState.Configure(HarvestState.WalkToResource)
                .OnEntry(() => OnWalkToResourceEnter())
                .Permit(HarvestTrigger.StartHarvestingResource, HarvestState.HarvestResource)
                .Permit(HarvestTrigger.StartUnstickingYourself, HarvestState.UnstickYourself)
                .Permit(HarvestTrigger.StartHarvest, HarvestState.Enter);

            _harvestState.Configure(HarvestState.HarvestResource)
                .OnEntry(() => OnHarvestResourceEnter())
                .Permit(HarvestTrigger.StartHarvest, HarvestState.Enter);

            // Mobs
            _harvestState.Configure(HarvestState.TravelToMob)
                .OnEntry(() => OnTravelToMobEnter())
                .Permit(HarvestTrigger.StartWalkingToMob, HarvestState.WalkToMob)
                .Permit(HarvestTrigger.StartUnstickingYourself, HarvestState.UnstickYourself)
                .Permit(HarvestTrigger.StartHarvest, HarvestState.Enter);

            _harvestState.Configure(HarvestState.WalkToMob)
                .OnEntry(() => OnWalkToMobEnter())
                .Permit(HarvestTrigger.StartAttackingMob, HarvestState.AttackMob)
                .Permit(HarvestTrigger.StartUnstickingYourself, HarvestState.UnstickYourself)
                .Permit(HarvestTrigger.StartHarvest, HarvestState.Enter);

            _harvestState.Configure(HarvestState.AttackMob)
                .OnEntry(() => OnAttackMobEnter())
                .Permit(HarvestTrigger.StartWalkingToResource, HarvestState.WalkToResource)
                .Permit(HarvestTrigger.StartHarvest, HarvestState.Enter);

            _harvestState.Configure(HarvestState.HarvestMob)
                .OnEntry(() => OnHarvestMobEnter())
                .Permit(HarvestTrigger.StartHarvest, HarvestState.Enter);

            // Being stuck sucks
            _harvestState.Configure(HarvestState.UnstickYourself)
                .OnEntry(() => OnUnstickYourselfEnter())
                .PermitReentry(HarvestTrigger.StartUnstickingYourself)
                .Permit(HarvestTrigger.StartMounting, HarvestState.Mounting)
                .Permit(HarvestTrigger.StartHarvest, HarvestState.Enter);
        }

        void HarvestUpdate()
        {
            if (HandleAttackers())
                return;

            switch (_harvestState.State)
            {
                case HarvestState.Enter: DoEnter(); break;
                case HarvestState.Mounting: DoMounting(); break;
                case HarvestState.TravelToResource: DoTravelToResource(); break;
                case HarvestState.WalkToResource: DoWalkToResource(); break;
                case HarvestState.HarvestResource: DoHarvestResource(); break;
                case HarvestState.TravelToMob: DoTravelToMob(); break;
                case HarvestState.WalkToMob: DoWalkToMob(); break;
                case HarvestState.AttackMob: DoAttackMob(); break;
                case HarvestState.HarvestMob: DoHarvestMob(); break;
                case HarvestState.UnstickYourself: DoUnstickYourself(); break;
            }
        }

        void OnHarvestEnter()
        {
            Core.Log("[Harvesting] -- OnHarvestEnter");
            Assert.IsTrue(_currentTarget is HarvestableObjectView || _currentTarget is MobView);

            if (!ValidateTarget(_currentTarget))
            {
                Core.Log("[Harvesting] - Resource Depleted, search for new one.");
                _state.Fire(Trigger.DepletedResource);
                return;
            }

            float distanceToPlayer = (_currentTarget.transform.position - _localPlayerCharacterView.transform.position).magnitude;

            if (_currentTarget is HarvestableObjectView obj)
            {
                if (distanceToPlayer < InteractRange)
                {
                    _harvestState.Fire(HarvestTrigger.StartWalkingToResource);
                }
                else
                {
                    if (!_localPlayerCharacterView.IsMounted)
                        _harvestState.Fire(HarvestTrigger.StartMounting);
                    else
                        _harvestState.Fire(HarvestTrigger.StartTravelingToResourceWithLittleWait);
                }
            }
            else if (_currentTarget is MobView)
            {
                if (distanceToPlayer < InteractRange)
                {
                    _harvestState.Fire(HarvestTrigger.StartWalkingToMob);
                }
                else
                {
                    if (!_localPlayerCharacterView.IsMounted)
                        _harvestState.Fire(HarvestTrigger.StartMounting);
                    else
                        _harvestState.Fire(HarvestTrigger.StartTravelingToMob);
                }
            }
        }

        void DoEnter()
        { }

        void OnMountingEnter()
        {
            Core.Log("[Harvesting] -- OnMountingEnter");

            if (!_localPlayerCharacterView.IsMounted)
                _localPlayerCharacterView.MountOrDismount();
        }

        void DoMounting()
        {
            Core.LogOnce("[Harvesting] -- DoMounting");

            StuckHelper.PretendPlayerIsMoving();
            if (_localPlayerCharacterView.IsMounted)
            {
                _harvestState.Fire(HarvestTrigger.StartHarvest);
            }
        }

        #region Resources
        void OnTravelToResourceEnter(bool waitALittle = false)
        {
            Core.Log("[Harvesting] -- OnTravelToResourceEnter");

            Assert.IsTrue(_currentTarget is HarvestableObjectView);

            Vector3 targetCenter = _currentTarget.transform.position;
            Vector3 playerCenter = _localPlayerCharacterView.transform.position;

            if (_localPlayerCharacterView.TryFindPath(new ClusterPathfinder(), targetCenter, IsBlockedGathering, out List<Vector3> pathing))
            {
                Core.Log("[Harvesting] - Path found, begin travel to resource.");
                Core.lineRenderer.positionCount = pathing.Count;
                Core.lineRenderer.SetPositions(pathing.ToArray());

                // Walk to resource 25% of the time.
                if (UnityEngine.Random.value < 0.25f)
                {
                    _harvestPathingRequest = new ClusterPathingRequest(_localPlayerCharacterView, _currentTarget, pathing,
                        UnityEngine.Random.Range(1.5f, 8f));
                }
                else
                {
                    _harvestPathingRequest = new ClusterPathingRequest(_localPlayerCharacterView, _currentTarget, pathing);
                }
            }
            else
            {
                Core.Log("[Harvesting] - Path not found.");
                _state.Fire(Trigger.DepletedResource);
            }

            if (waitALittle)
            {
                // Wait 25% of the time.
                if (UnityEngine.Random.value < 0.25f)
                {
                    _travelStartTime = DateTime.Now + TimeSpan.FromSeconds(UnityEngine.Random.value * _maxTravelWaitTime);
                }
            }
        }

        void DoTravelToResource()
        {
            Core.LogOnce("[Harvesting] -- DoTravelToResource");

            if (DateTime.Now < _travelStartTime)
            {
                StuckHelper.PretendPlayerIsMoving();
                return;
            }

            if (StuckHelper.IsPlayerStuck(_localPlayerCharacterView))
            {
                _harvestState.Fire(HarvestTrigger.StartUnstickingYourself);
                return;
            }

            if (!HandlePathing(ref _harvestPathingRequest))
                _harvestState.Fire(HarvestTrigger.StartWalkingToResource);
        }

        void OnWalkToResourceEnter()
        {
            Core.Log("[Harvesting] -- OnWalkToResourceEnter");

            HarvestableObjectView resource = _currentTarget as HarvestableObjectView;
            if (_localPlayerCharacterView.IsMounted)
                _localPlayerCharacterView.MountOrDismount();

            _localPlayerCharacterView.Interact(resource);
        }

        void DoWalkToResource()
        {
            Core.LogOnce("[Harvesting] -- DoWalkToResource");

            if (_localPlayerCharacterView.IsHarvesting())
            {
                _harvestState.Fire(HarvestTrigger.StartHarvestingResource);
                return;
            }

            if (StuckHelper.IsPlayerStuck(_localPlayerCharacterView))
            {
                _harvestState.Fire(HarvestTrigger.StartUnstickingYourself);
                return;
            }
        }

        void OnHarvestResourceEnter()
        {
            Core.Log("[Harvesting] -- OnHarvestResourceEnter");

            HarvestableObjectView resource = _currentTarget as HarvestableObjectView;
            var harvestableObject2 = resource.GetHarvestableObject();
            var resourceType = harvestableObject2.GetResourceType().Value;
            var tier = (Tier)harvestableObject2.GetTier();
            var enchantmentLevel = (EnchantmentLevel)harvestableObject2.GetRareState();

            var info = new GatherInformation(resourceType, tier, enchantmentLevel)
            {
                HarvestDate = DateTime.UtcNow
            };

            var position = resource.transform.position.c();
            if (_gatheredSpots.ContainsKey(position))
                _gatheredSpots[position] = info;
            else
                _gatheredSpots.Add(position, info);
        }

        void DoHarvestResource()
        {
            Core.LogOnce("[Harvesting] -- DoHarvestResource");

            StuckHelper.PretendPlayerIsMoving();
            HarvestableObjectView resource = _currentTarget as HarvestableObjectView;
            if (resource.GetHarvestableObject().GetCharges() <= 0)
            {
                Core.Log("[Harvesting] - Resource depleted, move on.");
                _state.Fire(Trigger.DepletedResource);
                return;
            }
        }
        #endregion Resources

        #region Mobs
        void OnTravelToMobEnter()
        {
            Core.Log("[Harvesting] -- OnTravelToMobEnter");

            Assert.IsTrue(_currentTarget is MobView);
            MobView mob = _currentTarget as MobView;

            Vector3 targetCenter = _currentTarget.transform.position;
            Vector3 playerCenter = _localPlayerCharacterView.transform.position;

            if (_localPlayerCharacterView.TryFindPath(new ClusterPathfinder(), targetCenter, IsBlockedGathering, out List<Vector3> pathing))
            {
                Core.Log("[Harvesting] - Path found, begin travel to mob,");
                Core.lineRenderer.positionCount = pathing.Count;
                Core.lineRenderer.SetPositions(pathing.ToArray());
                _harvestPathingRequest = new ClusterPathingRequest(_localPlayerCharacterView, _currentTarget, pathing);
            }
            else
            {
                Core.Log("[Harvesting] - Path not found.");
                _state.Fire(Trigger.DepletedResource);
            }
        }

        void DoTravelToMob()
        {
            Core.LogOnce("[Harvesting] -- DoTravelToMob");

            if (StuckHelper.IsPlayerStuck(_localPlayerCharacterView))
            {
                _harvestState.Fire(HarvestTrigger.StartUnstickingYourself);
                return;
            }

            Assert.IsTrue(_currentTarget is MobView);
            MobView mob = _currentTarget as MobView;

            Vector3 targetCenter = _currentTarget.transform.position;
            Vector3 playerCenter = _localPlayerCharacterView.transform.position;

            float centerDistance = (targetCenter - playerCenter).magnitude;
            var weaponItem = _localPlayerCharacterView.LocalPlayerCharacter.s7().o();
            var isMeleeWeapon = weaponItem == null || weaponItem.bu() == Albion.Common.GameData.AttackType.Melee;
            var attackRange = _localPlayerCharacterView.LocalPlayerCharacter.jz() + mob.Mob.w8().ew();
            var minimumAttackRange = isMeleeWeapon ? MeleeAttackRange : attackRange;
            var isInLoS = _localPlayerCharacterView.IsInLineOfSight(mob);

            if (!HandlePathing(ref _harvestPathingRequest, () => centerDistance <= minimumAttackRange && isInLoS))
                _harvestState.Fire(HarvestTrigger.StartWalkingToMob);
        }

        void OnWalkToMobEnter()
        {
            Core.Log("[Harvesting] -- OnWalkToMobEnter");

            MobView mob = _currentTarget as MobView;
            if (_localPlayerCharacterView.IsMounted)
                _localPlayerCharacterView.MountOrDismount();

            _localPlayerCharacterView.SetSelectedObject(mob);
            _localPlayerCharacterView.AttackSelectedObject();
        }

        void DoWalkToMob()
        {
            Core.LogOnce("[Harvesting] -- DoWalkToMob");

            MobView mob = _currentTarget as MobView;
            if (_localPlayerCharacterView.IsAttacking())
            {
                _harvestState.Fire(HarvestTrigger.StartAttackingMob);
                return;
            }

            if (StuckHelper.IsPlayerStuck(_localPlayerCharacterView))
            {
                _harvestState.Fire(HarvestTrigger.StartUnstickingYourself);
                return;
            }
        }

        void OnAttackMobEnter()
        {
            Core.Log("[Harvesting] -- OnAttackMobEnter");
        }

        void DoAttackMob()
        {
            Core.Log("[Harvesting] -- DoAttackMob");

            StuckHelper.PretendPlayerIsMoving();
            MobView mob = _currentTarget as MobView;
            if (mob.IsDead() && mob.DeadAnimationFinished)
            {
                Core.Log("[Harvesting] - Mob dead.");
                _harvestState.Fire(HarvestTrigger.StartWalkingToResource);
            }
        }

        void OnHarvestMobEnter()
        {
            Core.Log("[Harvesting] -- OnHarvestMobEnter");
        }

        void DoHarvestMob()
        {
            Core.LogOnce("[Harvesting] -- DoHarvestMob");

            StuckHelper.PretendPlayerIsMoving();
        }
        #endregion Mobs

        #region Sticky
        void OnUnstickYourselfEnter()
        {
            Core.Log("[Harvesting] -- OnUnstickYourselfEnter");

            if (!_localPlayerCharacterView.IsMounted)
            {
                _harvestState.Fire(HarvestTrigger.StartMounting);
                return;
            }

            // Chose a random point behind player.
            Vector2 back = new Vector2((-_localPlayerCharacterView.transform.forward).x, (-_localPlayerCharacterView.transform.forward).z) * 15f;
            float randAngle = UnityEngine.Random.Range(-75f, 75f);
            back = Quaternion.AngleAxis(randAngle, Vector3.up) * back;
            Vector3 randPos = new Vector3(back.x, 0f, back.y) + _localPlayerCharacterView.transform.position;

            _localPlayerCharacterView.CreateTextEffect("[Stuck detected - Resolving]");
            _localPlayerCharacterView.CreateTextEffect("x: " + randPos.x + " | z: " + randPos.z);

            _harvestPathingRequest = null;
            _localPlayerCharacterView.RequestMove(randPos);
            _unstickStartTime = DateTime.Now;
            //_currentTarget = null;
        }

        void DoUnstickYourself()
        {
            Core.Log("[Harvesting] -- DoUnstickYourself");

            if (StuckHelper.IsPlayerStuck(_localPlayerCharacterView))
            {
                _harvestState.Fire(HarvestTrigger.StartUnstickingYourself);
            }

            if (_unstickStartTime + _timeToUnstick < DateTime.Now)
            {
                _harvestState.Fire(HarvestTrigger.StartHarvest);
            }
        }
        #endregion Sticky

        #region Helpers
        bool ValidateHarvestable(HarvestableObjectView resource)
        {
            if (resource == null)
                return false;

            var resourceObject = resource.GetHarvestableObject();
            if (resourceObject == null)
                return false;

            if (!resourceObject.CanLoot(_localPlayerCharacterView)
                || resourceObject.GetCharges() <= 0
                || resourceObject.GetResourceDescriptor().Tier < (int)SelectedMinimumTier)
            {
                return false;
            }

            Vector3 position = resource.transform.position;
            float terrainHeight = _landscape.GetTerrainHeight(position.c(), out RaycastHit hit);

            if (position.y < terrainHeight - 5)
                return false;

            if (_blacklist.ContainsKey(resource))
                return false;

            //Skip if target is inside a kepper pack
            if (_currentTarget != null && _skipKeeperPacks && (ContainKeepers(_currentTarget.transform.position)))
            {
                Core.Log("[Harvesting] - Skipping resource, inside Keeper Pack Range");
                //Blacklist(resource, TimeSpan.FromMinutes(5));
                return false;
            }

            return true;
        }

        bool ValidateMob(MobView mob)
        {
            if (mob.IsDead())
                return false;

            var mobAttackTarget = mob.GetAttackTarget();
            if (mobAttackTarget != null && mobAttackTarget != _localPlayerCharacterView)
                return false;

            if (_blacklist.ContainsKey(mob))
                return false;

            return true;
        }

        bool ValidateTarget(SimulationObjectView target)
        {
            if (target is HarvestableObjectView resource)
                return ValidateHarvestable(resource);

            if (target is MobView mob)
                return ValidateMob(mob);

            return false;
        }
        #endregion Helpers
    }
}
