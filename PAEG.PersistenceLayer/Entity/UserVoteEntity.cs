using PAEG.Model;

namespace PAEG.PersistenceLayer.Entity;

public record UserVoteEntity(UserVote UserVote, DateTime DateTime = new DateTime());