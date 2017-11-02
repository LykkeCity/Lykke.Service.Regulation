// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Service.Regulation.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
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
        public NewWelcomeRegulationRuleModel(bool active, string country = default(string), string regulationId = default(string))
        {
            Country = country;
            RegulationId = regulationId;
            Active = active;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Country")]
        public string Country { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "RegulationId")]
        public string RegulationId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Active")]
        public bool Active { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            //Nothing to validate
        }
    }
}
