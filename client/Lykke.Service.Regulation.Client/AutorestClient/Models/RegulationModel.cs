// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Service.Regulation.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class RegulationModel
    {
        /// <summary>
        /// Initializes a new instance of the RegulationModel class.
        /// </summary>
        public RegulationModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the RegulationModel class.
        /// </summary>
        public RegulationModel(string id = default(string))
        {
            Id = id;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

    }
}
