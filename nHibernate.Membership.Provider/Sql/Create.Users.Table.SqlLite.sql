CREATE TABLE User
(
  ID GUID NOT NULL PRIMARY KEY,
  Username Text (255) NOT NULL,
  ApplicationName Text (255) NOT NULL,
  Email Text (128) NOT NULL,
  Comment Text (255),
  Password Text (128) NOT NULL,
  PasswordQuestion Text (255),
  PasswordAnswer Text (255),
  IsApproved YesNo,
  LastActivityDate DateTime,
  LastLoginDate DateTime,
  LastPasswordChangedDate DateTime,
  CreationDate DateTime,
  IsOnLine YesNo,
  IsLockedOut YesNo,
  LastLockedOutDate DateTime,
  FailedPasswordAttemptCount Integer,
  FailedPasswordAttemptWindowStart DateTime,
  FailedPasswordAnswerAttemptCount Integer,
  FailedPasswordAnswerAttemptWindowStart DateTime
)