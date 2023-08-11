namespace VideoProcessor.DTO.OutputDtos
{
    /// <summary>
    /// Output DTO that contains links to videos from different processing stages
    /// </summary>
    public class VideoLocations
    {
        /// <summary>
        /// The orginal name of the video
        /// </summary>
        public string VideoName { get; set; }

        /// <summary>
        /// The location of the video after transcoding
        /// </summary>
        public string TranscodeLocation { get; set; }

        /// <summary>
        /// The location of the video after thumbnail extracted
        /// </summary>
        public string ThumbnailLocation { get; set; }

        /// <summary>
        /// The location of the completed video
        /// </summary>
        public string CompletedLocation { get; set; }
    }
}
