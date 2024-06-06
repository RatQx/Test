export function calculateTimeDifference(
  startTime: string,
  endTime: string
): string {
  const currentTime = new Date().getTime();
  const auctionStartTime = new Date(startTime).getTime();
  const auctionEndTime = new Date(endTime).getTime();

  if (currentTime < auctionStartTime) {
    return 'Auction not started yet';
  } else if (currentTime >= auctionStartTime && currentTime <= auctionEndTime) {
    const diff = auctionEndTime - currentTime;

    const millisecondsPerSecond = 1000;
    const millisecondsPerMinute = 60 * millisecondsPerSecond;
    const millisecondsPerHour = 60 * millisecondsPerMinute;
    const millisecondsPerDay = 24 * millisecondsPerHour;

    const days = Math.floor(diff / millisecondsPerDay);
    const hours = Math.floor((diff % millisecondsPerDay) / millisecondsPerHour);
    const minutes = Math.floor(
      (diff % millisecondsPerHour) / millisecondsPerMinute
    );
    const seconds = Math.floor(
      (diff % millisecondsPerMinute) / millisecondsPerSecond
    );

    return `${days} days, ${hours} hours, ${minutes} minutes, ${seconds} seconds`;
  } else {
    return 'Auction ended';
  }
}
