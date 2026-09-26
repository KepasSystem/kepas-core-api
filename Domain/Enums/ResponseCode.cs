namespace Kepas.Core.Api.Domain.Enums
{
    public enum ResponseCode
    {
        SUCCESS,
        CREATED,
        UPDATED,
        DELETED,
        
        COMPANY_NOT_FOUND,
        COMPANY_SUSPENDED,
        
        LOGIN_SUCCESSFUL,
        INVALID_CREDENTIALS,
        USER_INACTIVE,
        UNAUTHORIZED,
        FORBIDDEN,
        
        VALIDATION_ERROR,
        INTERNAL_SERVER_ERROR,
        PAGE_NOT_FOUND,
        
        SMTP_UPDATED_SUCCESS,
        QR_CODE_GENERATED
    }
}
