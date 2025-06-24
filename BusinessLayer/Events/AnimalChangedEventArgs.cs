using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Events
{
    public enum AnimalChangeType
    {
        Added,
        Updated,
        Deleted
    }
    public class AnimalChangedEventArgs : EventArgs
    {
        public AnimalDto Animal { get; }
        public AnimalChangeType ChangeType { get; }

        public AnimalChangedEventArgs(AnimalDto animal, AnimalChangeType changeType)
        {
            Animal = animal ?? throw new ArgumentNullException(nameof(animal));
            ChangeType = changeType;
        }
    }

}
