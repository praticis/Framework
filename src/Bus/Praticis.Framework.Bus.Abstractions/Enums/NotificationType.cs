
namespace Praticis.Framework.Bus.Abstractions.Enums
{
    /// <summary>
    /// The application event types. <strong>**IMPORTANT**</strong> If extends this class, 
    /// use event code greather than 100 to prevent conflits with possible new event codes 
    /// of the Praticis Framework.
    /// </summary>
    public class NotificationType
    {
        /// <summary>
        /// Representas a default notification.
        /// <strong>**IMPORTANT**</strong> If extends this class, use event code greather than 100 
        /// to prevent conflits with possible new event codes of the Praticis Framework.
        /// </summary>
        public const int Default = 0;

        /// <summary>
        /// Represents a domain notification.
        /// <strong>**IMPORTANT**</strong> If extends this class, use event code greather than 100 
        /// to prevent conflits with possible new event codes of the Praticis Framework.
        /// </summary>
        public const int Domain_Notification = 1;

        /// <summary>
        /// Represents warning notification. A message that need be show to User.
        /// <strong>**IMPORTANT**</strong> If extends this class, use event code greather than 100 
        /// to prevent conflits with possible new event codes of the Praticis Framework.
        /// </summary>
        public const int Warning = 2;

        /// <summary>
        /// Represents an entry system log.
        /// <strong>**IMPORTANT**</strong> If extends this class, use event code greather than 100 
        /// to prevent conflits with possible new event codes of the Praticis Framework.
        /// </summary>
        public const int Log = 3;

        /// <summary>
        /// Represents a system error. Contains sensitive informations and this can't be show to user.
        /// <strong>**IMPORTANT**</strong> If extends this class, use event code greather than 100 
        /// to prevent conflits with possible new event codes of the Praticis Framework.
        /// </summary>
        public const int System_Error = 4;

        /// <summary>
        /// Represents that an event will be stored.
        /// <strong>**IMPORTANT**</strong> If extends this class, use event code greather than 100 
        /// to prevent conflits with possible new event codes of the Praticis Framework.
        /// </summary>
        public const int Stored_Event = 5;

        /// <summary>
        /// Represents a pipline finished execution.
        /// <strong>**IMPORTANT**</strong> If extends this class, use event code greather than 100 
        /// to prevent conflits with possible new event codes of the Praticis Framework.
        /// </summary>
        public const int Pipeline_Finished = 6;

        /// <summary>
        /// Represents a created work in worker module and waiting to enqueued after.
        /// <strong>**IMPORTANT**</strong> If extends this class, use event code greather than 100 
        /// to prevent conflits with possible new event codes of the Praticis Framework.
        /// </summary>
        public const int Work_Created = 7;

        /// <summary>
        /// Represents a enqueued work in worker module.
        /// <strong>**IMPORTANT**</strong> If extends this class, use event code greather than 100 
        /// to prevent conflits with possible new event codes of the Praticis Framework.
        /// </summary>
        public const int Work_Enqueued = 8;

        /// <summary>
        /// Represents a start execution in worker module.
        /// <strong>**IMPORTANT**</strong> If extends this class, use event code greather than 100 
        /// to prevent conflits with possible new event codes of the Praticis Framework.
        /// </summary>
        public const int Work_Started = 9;

        /// <summary>
        /// Represents a finished execution successfully in worker module.
        /// <strong>**IMPORTANT**</strong> If extends this class, use event code greather than 100 
        /// to prevent conflits with possible new event codes of the Praticis Framework.
        /// </summary>
        public const int Work_Finished = 10;

        /// <summary>
        /// Represents a finished execution failed in worker module.
        /// <strong>**IMPORTANT**</strong> If extends this class, use event code greather than 100 
        /// to prevent conflits with possible new event codes of the Praticis Framework.
        /// </summary>
        public const int Work_Failed = 11;
    }
}