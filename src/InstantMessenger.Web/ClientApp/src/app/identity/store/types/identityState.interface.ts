import {CurrentUserInterface} from '../../../shared/types/currentUser.interface';
export interface IdentityStateInterface {
  isSubmitting: boolean;
  currentUser: CurrentUserInterface | null;
}
