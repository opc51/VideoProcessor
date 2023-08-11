namespace VideoProcessor.DTO.Inputs
{
    /// <summary>
    /// The input DTO that is used to begin processing a raw video into release format
    /// </summary>
    public class InputVideoDto
    {
        /// <summary>
        /// The name of the provided video
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The duration of the video
        /// </summary>
        public int Duration { get; set; }
    }
}
