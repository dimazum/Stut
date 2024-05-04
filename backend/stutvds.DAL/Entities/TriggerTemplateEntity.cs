using StopStatAuth_6_0.Entities.Base;
using stutvds.DAL.Entities;

namespace StopStatAuth_6_0.Entities
{
    public class TriggerTemplateEntity : Entity
    {
        public TriggerEntity Trigger { get;set; }
        public TwisterTemplateEntity Template { get; set; }
    }
}
