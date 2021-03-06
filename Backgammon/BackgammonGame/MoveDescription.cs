﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonGame
{
    public enum MoveDirection
    {
        Left,
        Right
    }

    public interface IMoveDescription
    {
        int From { get; set; }

        int To { get; set; }

        MoveDirection Direction { get; }

        PlayerId PlayerId { get; set; }
    }

    internal class MoveDescription : IMoveDescription, IEquatable<MoveDescription>
    {
        public MoveDescription(int from, int to, MoveDirection direction, PlayerStatus playerStatus, PlayerId playerId)
        {
            From = from;
            To = to;
            Direction = direction;
            PlayerStatus = playerStatus;
            PlayerId = playerId;
        }

        public int From { get; set; }

        public int To { get; set; }

        public MoveDirection Direction { get; set; }

        public PlayerStatus PlayerStatus { get; set; }

        public PlayerId PlayerId { get; set; }

        public override bool Equals(object obj)
        {
            var move = obj as MoveDescription;

            if(obj == null)
            {
                return false;
            }

            return Equals(move);
        }

        public bool Equals(MoveDescription other)
        {
            if(other == null)
            {
                return false;
            }

            if(PlayerId != other.PlayerId)
            {
                return false;
            }

            if(Direction != other.Direction)
            {
                return false;
            }

            return From == other.From && To == other.To;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
