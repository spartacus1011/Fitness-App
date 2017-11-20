using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_tracker.Classes
{
    public class ExerciseHistoryItem 
    {
        public string ExerciseGUIDID { get; set; } 
        public bool IsRep { get; set; } //is rep or set I.E partial or full 
        public DateTime TimeCompleted { get; set; }
        //public string PlaceCompleted {get;set;} //future plans?

        public ExerciseHistoryItem(string guidid, bool isRep, DateTime timeCompleted) //i dont like putting something in the constructor but it might be the only way. This would work fine if i do this as a database...
        {
            ExerciseGUIDID = guidid;
            IsRep = isRep;
            TimeCompleted = timeCompleted;
        }



    }
}
