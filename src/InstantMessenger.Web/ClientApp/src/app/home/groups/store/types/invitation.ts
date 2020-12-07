export enum ExpirationTimeType {
  INFINITE = 'Infinite',
  BOUNDED = 'Bounded',
}

export enum UsageCounterType {
  INFINITE = 'Infinite',
  BOUNDED = 'Bounded',
}

export interface GenerateInvitationRequest {
  groupId: string;
  invitationId: string;
  expirationTime: {
    type: ExpirationTimeType;
    period: string;
  };
  usageCounter: {
    type: UsageCounterType;
    times: number;
  };
}

export interface InvitationDto {
  groupId: string;
  invitationId: string;
  code: string;
  expirationTime: {
    type: ExpirationTimeType;
    start: string;
    period: string;
  };
  usageCounter: {
    type: UsageCounterType;
    value: number;
  };
}
