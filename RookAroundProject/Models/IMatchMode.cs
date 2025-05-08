    namespace RookAroundProject{
        public interface IMatchMode{
            string Title {get; set;}
            List<Resource> Resources {get;}
            int RegularPlayers {get;}
            bool HasGMPlayer {get;}

        }

    }
