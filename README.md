# Revenge of the Roombas

## Backend: PHP Leaderboard Service

While this repo started as a Unity game jam project, it also includes a small **PHP + SQLite backend** (`/backend`) that I built to power an online leaderboard, along with the C# client code in the game that talks to it.

- **REST-ish API** (`backend/public/index.php`) — a single `/scores` endpoint: `GET` returns the top 10 scores, `POST` submits a new one.
- **Request authentication** (`backend/src/signature.php`) — score submissions are signed with an HMAC-SHA256 of `initials:score:nonce`, verified server-side with `hash_equals` to avoid timing attacks.
- **Replay protection** — each submission carries a one-time nonce; the server tracks used nonces in SQLite (`used_nonces` table) and rejects duplicates, including a race-safe insert that treats a `UNIQUE constraint` violation as "already used" rather than an error.
- **Input validation** — all fields (initials, score, nonce, signature) are strictly type- and format-checked before touching the database.
- **Persistence** (`backend/src/db.php`) — a lightweight `PDO`/SQLite layer handling schema setup, ranked top-10 queries, and rank computation for a given submission.
- **Config kept out of git** — `backend/config.php` (holding the HMAC secret and db path) is gitignored; only `config.php.example` is committed, with setup steps in `backend/README.md`.

See [`backend/README.md`](backend/README.md) for the full API spec and setup instructions.

---

A long time ago, in a living room far, far way, a gang of Roombas went haywire and rebelled against their household. The great vacuum robot, Murphy, decided to take on the challenge of putting the wild Roombas back in their place. Will Murphy succeed? Or will he be defeated, leaving the rest of the world to the hands of the maniacal Roombas?

**[Play it on itch.io](https://amendez.itch.io/revenge-of-the-roombas)**

![Revenge of the Roombas cover art](https://img.itch.zone/aW1nLzg0NjM5NDIuanBn/original/Bc%2FS4z.jpg)

## Screenshots

<table>
<tr>
<td><img src="https://img.itch.zone/aW1hZ2UvMTQ0OTUwNC84NDY2MDk3LmpwZw==/original/KFhtCe.jpg" alt="Screenshot 1" width="400"></td>
<td><img src="https://img.itch.zone/aW1hZ2UvMTQ0OTUwNC84NDY2MDk4LmpwZw==/original/mZG9LY.jpg" alt="Screenshot 2" width="400"></td>
</tr>
<tr>
<td><img src="https://img.itch.zone/aW1hZ2UvMTQ0OTUwNC84NDY2MDk5LmpwZw==/original/QtdG0h.jpg" alt="Screenshot 3" width="400"></td>
<td><img src="https://img.itch.zone/aW1hZ2UvMTQ0OTUwNC84NDY2MTAwLmpwZw==/original/W56pJy.jpg" alt="Screenshot 4" width="400"></td>
</tr>
<tr>
<td><img src="https://img.itch.zone/aW1hZ2UvMTQ0OTUwNC84NDY2MTAxLmpwZw==/original/RwlxiR.jpg" alt="Screenshot 5" width="400"></td>
<td><img src="marketing/leaderboard.png" alt="High scores leaderboard" width="400"></td>
</tr>
</table>

## About

Revenge of the Roombas is an arcade-style, isometric fighting/action game built for the UTD SGDA game jam (Spring 2022), developed in about 10 days using Unity. Fight through Roombas armed with weapons and explosives and rack up as high a score as you can.

- **Genre:** Action, fighting, arcade, isometric, 3D
- **Platform:** Windows
- **Engine:** Unity

## Controls

| Action | Key |
| --- | --- |
| Move | W / A / S / D |
| Dash | O |
| Punch | P |

## Credits

- Connor Boone — Animation/Rigging
- Nick Maclean — Programming
- Ariana Mendez — Environment Design
- Kellyn Mendez — Programming
- Wesley Pate — Audio/Game Design

