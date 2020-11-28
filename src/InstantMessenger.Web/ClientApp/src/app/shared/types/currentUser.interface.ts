type Guid = string;

export interface CurrentUserInterface {
  id: Guid;
  email: string;
  createdAt: string;
  token: string;
}
