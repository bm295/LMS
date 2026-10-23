from datetime import datetime, timedelta


def calculate_deadline(start: datetime, standard_days: int, has_extension: bool) -> datetime:
    """Count weekdays after the start date; an extension adds seven weekdays."""
    if standard_days <= 0:
        raise ValueError("Standard days must be positive.")
    remaining = standard_days + (7 if has_extension else 0)
    deadline = start
    # Skip complete weeks to avoid a loop proportional to the requested duration.
    if deadline.weekday() >= 5:
        deadline += timedelta(days=7 - deadline.weekday())
        remaining -= 1
    weeks, days = divmod(remaining, 5)
    deadline += timedelta(days=weeks * 7)
    while days:
        deadline += timedelta(days=1)
        if deadline.weekday() < 5:
            days -= 1
    return deadline
