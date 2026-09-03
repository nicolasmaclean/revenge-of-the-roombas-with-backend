<?php
  class Database
  {
    private array $config;
    private $pdo;
    
    public function __construct(array $config)
    {
      $this->config = $config;

      // connect to db
      $dsn = "sqlite:" . $config["db_path"];
      $this->pdo = $pdo = new \PDO($dsn);
      $pdo->setAttribute(\PDO::ATTR_ERRMODE, \PDO::ERRMODE_EXCEPTION);

      // setup tables
      $pdo->exec('CREATE TABLE IF NOT EXISTS scores (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        initials TEXT NOT NULL,       -- 3 chars
        score INTEGER NOT NULL,       -- non-negative
        created_at TEXT NOT NULL      -- ISO 8601, server-generated
      )');
      $pdo->exec('CREATE INDEX IF NOT EXISTS idx_scores_score ON scores(score desc)');
      $pdo->exec('CREATE TABLE IF NOT EXISTS used_nonces (
        nonce TEXT PRIMARY KEY,
        created_at TEXT NOT NULL
      )');
    }

    public function getTop(int $limit = 10): array
    {
      $query = $this->pdo->prepare('
        SELECT initials, score, created_at
        FROM scores 
        ORDER BY score
        DESC LIMIT ?
      ');
      $query->bindValue(1, $limit, PDO::PARAM_INT);
      $query->execute();
      return $query->fetchAll(PDO::FETCH_ASSOC);
    }

    public function insert(string $name, int $score): int
    {
      $query = $this->pdo->prepare('
        INSERT INTO scores (initials, score, created_at) VALUES (?, ?, ?)
      ');
      $query->execute([$name, $score, gmdate('Y-m-d\TH:i:s\Z')]);
      return $this->pdo->lastInsertId();
    }

    public function rankFor(int $scoreId): int
    {
      $query = $this->pdo->prepare(
        'SELECT COUNT(*) + 1 FROM scores WHERE score > (SELECT score FROM scores WHERE id = ?)'
      );
      $query->execute([$scoreId]);
      return (int) $query->fetchColumn();
    }

    public function nonceExists(string $nonce): bool
    {
      $query = $this->pdo->prepare('SELECT 1 FROM used_nonces WHERE nonce = ?');
      $query ->execute([$nonce]);
      return $query->fetchColumn() !== false;
    }

    public function recordNonce(string $nonce): void
    {
      try
      {
        $query = $this->pdo->prepare(
          'INSERT INTO used_nonces (nonce, created_at) VALUES (?, ?)'
        );
        $query->execute([$nonce, gmdate('Y-m-d\TH:i:s\Z')]);
      }
      catch (PDOException $e)
      {
        // Unique constraint violation means a concurrent request already
        // recorded this nonce — treat it as already used, not an error.
        if (!str_contains($e->getMessage(), 'UNIQUE constraint failed'))
        {
          throw $e;
        }
      }
    }
  }

