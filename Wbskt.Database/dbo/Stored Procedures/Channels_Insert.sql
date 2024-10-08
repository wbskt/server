﻿/* -------------------------------- */
/* Channels_Insert                  */
/* Author: Richard Joy              */
/* Updated by: Richard Joy          */
/* Create date: 24-Aug-2024         */
/* Description: Self explanatory    */
/* -------------------------------- */
CREATE PROCEDURE dbo.Channels_Insert
(
  @Id			        INT OUTPUT
, @ChannelName	        VARCHAR(100)
, @UserId		        INT
, @ServerId             INT
, @ChannelPublisherId	UNIQUEIDENTIFIER
, @ChannelSubscriberId	UNIQUEIDENTIFIER
, @RetentionTime        INT
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Channels
    ( ChannelName
    , UserId
    , ChannelPublisherId
    , ChannelSubscriberId
    , RetentionTime
    , ServerId
    )
    VALUES
        ( @ChannelName
        , @UserId
        , @ChannelPublisherId
        , @ChannelSubscriberId
        , @RetentionTime
        , @ServerId
        );
    SELECT @Id = SCOPE_IDENTITY();
END;