import {HttpErrorResponse, HttpResponse} from '@angular/common/http';

export type ErrorResponseInterface =
  | ExpectedErrorInterface
  | UnexpectedErrorInterface;
export interface ExpectedErrorInterface {
  kind: 'expected_error';
  code: string;
  message: string;
  statusCode: number;
}
export interface UnexpectedErrorInterface {
  kind: 'unexpected_error';
  message: string;
}
export function mapToError(
  response: HttpErrorResponse
): ErrorResponseInterface {
  if (isExpectedError(response.error)) {
    return {
      kind: 'expected_error',
      code: response.error.code,
      message: response.error.message,
      statusCode: response.status,
    };
  } else {
    return {
      kind: 'unexpected_error',
      message: 'There is problem with server connection.',
    };
  }
}

function isExpectedError(object: any): object is ExpectedErrorInterface {
  if (typeof object === 'undefined') return false;
  return 'code' in object && 'message' in object;
}
