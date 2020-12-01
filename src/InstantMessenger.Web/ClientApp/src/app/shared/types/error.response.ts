import {HttpErrorResponse, HttpResponse} from '@angular/common/http';

export interface ExpectedErrorInterface {
  code: string;
  message: string;
  statusCode: number;
}
function isExpectedError(object: any): object is ExpectedErrorInterface {
  return 'code' in object && 'message' in object;
}
export interface UnexpectedErrorInterface {
  message: string;
}

export type ErrorResponseInterface =
  | ExpectedErrorInterface
  | UnexpectedErrorInterface;

export function mapToError(
  response: HttpErrorResponse
): ErrorResponseInterface {
  if (isExpectedError(response.error)) {
    return {
      code: response.error.code,
      message: response.error.message,
      statusCode: response.status,
    };
  } else {
    return {
      message: 'Some unexpected error occurred',
    };
  }
}
