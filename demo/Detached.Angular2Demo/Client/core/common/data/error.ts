
export interface IError {
    code: string;
    message: string;
}

export class ApiError implements IError {
    public code: string;
    public message: string;
    public debugInfo: any;
}

export class ApiValidationError extends ApiError implements IError {
    public members: Map<string, string>;
}

export class ApiErrorCodes {
    public static UnauthorizedAccess: string = "UnauthorizedAccess";
    public static InternalError: string = "InternalError";
    public static InvalidModel: string = "InvalidModel";
    public static NotFound: string = "NotFound";
    public static NoInternetConnection: string = "NoInternetConnection";
}