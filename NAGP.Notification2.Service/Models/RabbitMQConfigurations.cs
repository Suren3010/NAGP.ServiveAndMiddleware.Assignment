namespace NAGP.Notification2.Service
{
    /// <summary>
    /// Model for rabbit mq configurations 
    /// </summary>
    public class RabbitMQConfigurations
    {
        /// <summary>
        /// RabbitMQ hostname
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// RabbitMq user name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// RabbitMq Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// RabbitMq porta
        /// </summary>
        public int Port { get; set; }
    }
}
