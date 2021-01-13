import {ExpectedErrorInterface} from 'src/app/shared/types/error.response';

const codesMap = {
  insufficient_permissions:
    "You cannot performe this action, since you don't have proper permissions.",
  invalid_invitation_code:
    'Given invitation code is invalid. Please verify it and try again.',
  invalid_invitation:
    'Invitation for given code is not valid any more. Please use different invitation code.',
  internal_error:
    'Sorry, but some unexpected error occuried on server. Please try again.',
};
export function mapToMessage(error: ExpectedErrorInterface): string {
  if (codesMap[error.code]) {
    return codesMap[error.code];
  } else {
    return error.message;
  }
}
