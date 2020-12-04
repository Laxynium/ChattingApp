export interface UserInterface {
  id: string;
  nickname: string;
  avatar: string;
}

export function withDefaultAvatar(user: UserInterface) {
  if (!user) {
    return user;
  }
  return {
    ...user,
    avatar: user.avatar ? user.avatar : 'assets/profile-placeholder.png',
  };
}
