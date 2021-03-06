export enum ActionTypes {
  SIGN_UP = '[Identity] Sign Up',
  SIGN_UP_SUCCESS = '[Identity] Sign up success',
  SIGN_UP_FAILURE = '[Identity] Sign up failure',

  ACTIVATE = '[Identity] Activate',
  ACTIVATE_SUCCESS = '[Identity] Activate success',
  ACTIVATE_FAILURE = '[Identity] Activate failure',

  SIGN_IN = '[Identity] Sign in',
  SIGN_IN_SUCCESS = '[Identity] Sign in success',
  SIGN_IN_FAILURE = '[Identity] Sign in failure',

  FORGOT_PASSWORD = '[Identity] Forgot password',
  FORGOT_PASSWORD_SUCCESS = '[Identity] Forgot password success',
  FORGOT_PASSWORD_FAILURE = '[Identity] Forgot password failure',

  RESET_PASSWORD = '[Identity] Reset password',
  RESET_PASSWORD_SUCCESS = '[Identity] Reset password success',
  RESET_PASSWORD_FAILURE = '[Identity] Reset password failure',

  LOGOUT = '[Identity] Logout',
  LOGOUT_SUCCESS = '[Identity] Logout success',
  LOGOUT_FAILURE = '[Identity] Logout failure',

  CHANGE_NICKNAME = '[Identity] Change nickname',
  CHANGE_NICKNAME_SUCCESS = '[Identity] Change nickname success',

  UPLOAD_AVATAR = '[Identity] Upload avatar',
  UPLOAD_AVATAR_SUCCESS = '[Identity] Upload avatar success',
  UPLOAD_AVATAR_FAILURE = '[Identity] Upload avatar failure',

  GET_CURRENT_USER = '[Identity] Get current user',
  GET_CURRENT_USER_SUCCESS = '[Identity] Get current user success',
  GET_CURRENT_USER_FAILURE = '[Identity] Get current user success',
}
