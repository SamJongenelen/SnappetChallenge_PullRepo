using System;
using System.Collections.Generic;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    [RoutePrefix("api")]
    public class ProgressController : ApiController
    {
        [HttpGet]
        public object GetAllProgress()
        {
            //note: working stubdata:
            //var bla = "{                 \"sEcho\": 3,     \"iTotalRecords\": 57,     \"iTotalDisplayRecords\": 57,     \"aaData\": [         {             \"DT_RowId\": \"row_7\",             \"DT_RowClass\": \"gradeA\",             \"0\": \"Gecko\",             \"1\": \"Firefox 1.0\",             \"2\": \"Win 98+ / OSX.2+\",             \"3\": \"1.7\",             \"4\": \"A\"         },         {             \"DT_RowId\": \"row_8\",             \"DT_RowClass\": \"gradeA\",             \"0\": \"Gecko\",             \"1\": \"Firefox 1.5\",             \"2\": \"Win 98+ / OSX.2+\",             \"3\": \"1.8\",             \"4\": \"A\"         }            ] }";
            //JObject json = JObject.Parse(bla);
            //return json;
            
            var dt = new DateTime(2015, 3, 24, 11, 30, 0);
            DateTime dateTimeEnd = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            DateTime dateTimeStart = DateTime.SpecifyKind(dt.AddDays(-7), DateTimeKind.Utc);

            var model = ProgressBetween(dateTimeStart, dateTimeEnd);

            var dtr = new DataTableResult<Progress>();
            dtr.aaData = model;
            dtr.sEcho = 3;
            dtr.iTotalRecords = model.Count;
            dtr.iTotalDisplayRecords = 2;

            return dtr;
        }

        public List<Progress> ProgressBetween(DateTime dateStart, DateTime dkateEnd)
        {
            var progressList = new List<Progress>();
            progressList.Add(new Progress { CorrectAnswerRate = 1, DifficultyOfExercises = 2, NumberOfExercises = 3, Progresss = 1 });
            progressList.Add(new Progress { CorrectAnswerRate = 2, DifficultyOfExercises = 3, NumberOfExercises = 1, Progresss = 1 });

            //toto: get progress
            //order by worst performance

            return progressList;
        }
    }
}
