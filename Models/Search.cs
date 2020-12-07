using QuestionServiceWebApi.Db.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionServiceWebApi.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 

    public class Search : IEntity
    {
        public List<Question> items { get; set; }
        public bool has_more { get; set; }
        public int quota_max { get; set; }
        public int quota_remaining { get; set; }
        public int Id { get; set; }
    }
    public class Owner:IEntity
    {
        public int Id { get; set; }
        public int reputation { get; set; }
        public int user_id { get; set; }
        public string user_type { get; set; }
        public string profile_image { get; set; }
        public string display_name { get; set; }
        public string link { get; set; }
        public int? accept_rate { get; set; }
    }

    public class Question:IEntity
    {
        /// <summary>
        /// auto index
        /// </summary>
        public int Id { get; set; }
        [Column("tags")]
        public List<string> Tags { get; set; }
        public Owner Owner { get; set; }
        public bool is_answered { get; set; }
        public int view_count { get; set; }
        public int answer_count { get; set; }
        public int score { get; set; }
        public int last_activity_date { get; set; }
        public int creation_date { get; set; }
        public int last_edit_date { get; set; }
        public int question_id { get; set; }
        public string content_license { get; set; }
        public string link { get; set; }
        public string title { get; set; }
        public int? bounty_amount { get; set; }
        public int? bounty_closes_date { get; set; }
        public int? closed_date { get; set; }
        public string closed_reason { get; set; }
    }

 


}
