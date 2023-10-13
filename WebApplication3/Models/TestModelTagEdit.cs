using DBLayer.Models;

namespace WebApplication3.Models
{
    public class TestModelTagEdit
    {
        public Test Post { get; set; }
        public List<Tags> PostTags { get; set; }
        public List<int> SelectedTagIds { get; set; }
    }
}
