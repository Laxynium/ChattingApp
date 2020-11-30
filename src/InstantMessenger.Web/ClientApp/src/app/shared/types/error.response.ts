import {HttpErrorResponse, HttpResponse} from '@angular/common/http';

export interface ErrorResponseInterface {
  code: string;
  message: string;
  statusCode: number;
}

export function mapToError(
  response: HttpErrorResponse
): ErrorResponseInterface {
  return {
    code: response.error.code,
    message: response.error.message,
    statusCode: response.status,
  };
}
