﻿
namespace Domain0.Repository.Model
{
    public class MessageTemplate
    {
        public int Id { get; set; }

        public string Locale { get; set; }

        public string Type { get; set; }

        public string Template { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int EnvironmentId { get; set; }
    }

    public enum MessageTemplateName
    {
        WelcomeTemplate,
        WelcomeSubjectTemplate,
        RegisterSubjectTemplate,
        RegisterTemplate,
        RequestResetSubjectTemplate,
        RequestResetTemplate,
        ForcePasswordResetSubjectTemplate,
        ForcePasswordResetTemplate,

        RequestPhoneChangeTemplate,

        RequestEmailChangeTemplate,
        RequestEmailChangeSubjectTemplate,
    }

    public enum MessageTemplateType
    {
        sms, email
    };

}
