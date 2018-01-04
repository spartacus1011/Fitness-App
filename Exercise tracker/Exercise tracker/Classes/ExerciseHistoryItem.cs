using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercise_tracker.Helpers;

namespace Exercise_tracker.Classes
{
    //This is really a struct
    public class ExerciseHistoryItem 
    {
        public string ExerciseGUIDID { get; set; } 
        public bool IsRep { get; set; } //is rep or set I.E partial or full 
        public DateTime TimeCompleted { get; set; }
        //public string PlaceCompleted {get;set;} //future plans?
        public int RepsDone { get; set; }
        public int TimeDone { get; set; }
        public int ExerciseTimeUnitsId { get; set; }

        public ExerciseHistoryItem(string guidid, bool isRep, DateTime timeCompleted, int repsDone, int timeDone, int exerciseTimeUnitsId) //i dont like putting something in the constructor but it might be the only way. This would work fine if i do this as a database...
        {
            ExerciseGUIDID = guidid;
            IsRep = isRep;
            TimeCompleted = timeCompleted;
            RepsDone = repsDone;
            TimeDone = timeDone;
            ExerciseTimeUnitsId = exerciseTimeUnitsId;
        }

    }
}
