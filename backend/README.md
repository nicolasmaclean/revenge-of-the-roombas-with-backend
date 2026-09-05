# Leaderboard Backend

We've got a plain-old php server here in the backend.

## Endpoints

### /scores

#### GET

Get the top ten scores from the leaderboard right now.

Example output: {
    "scores": [
        {
            "initials": "NIC",
            "score":4820,
            "created_at":"2026-09-03T15:37:43Z"
        }
    ]
}

#### POST

Insert a new score for the leaderboard, then get information about the update.

Input: {
    "initials": "IAN",
    "score": 25000,
    "nonce": 32 char nonce,
    "signature": 64 char HMAC of 'initials:score:nonce'
}

Output: {
    "scores": [
        {
            "initials": "IAN",
            "score": 25000,
            "created_at":"2026-09-03T15:37:43Z"
        }
    ],
    "accepted": true,
    "rank": 1
}

## Setup

1. Copy config.php:

```
cp config.php.example config.php
```

2. In config.php, replace `CHANGE-ME!` with a real random secret:

```
php -r "echo bin2hex(random_bytes(32));"
```

3. Run the server

```
php -S localhost:8080 -t public
```

## Notes

HMAC signing only deters casual tampering since the secret will also need to ship with the client binary.

