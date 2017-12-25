// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Service.Regulation.Client.AutorestClient.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class NewWelcomeRegulationRuleModel
    {
        /// <summary>
        /// Initializes a new instance of the NewWelcomeRegulationRuleModel
        /// class.
        /// </summary>
        public NewWelcomeRegulationRuleModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the NewWelcomeRegulationRuleModel
        /// class.
        /// </summary>
        public NewWelcomeRegulationRuleModel(string name, IList<string> countries, string regulationId, bool active, int priority)
        {
            Name = name;
            Countries = countries;
            RegulationId = regulationId;
            Active = active;
            Priority = priority;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Countries")]
        public IList<string> Countries { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "RegulationId")]
        public string RegulationId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Active")]
        public bool Active { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Priority")]
        public int Priority { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Name == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Name");
            }
            if (Countries == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Countries");
            }
            if (RegulationId == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "RegulationId");
            }
        }
    }
}
