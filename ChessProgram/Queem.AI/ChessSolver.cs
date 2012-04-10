using System;
using Queem.Core;
using Queem.Core.ChessBoard;
using Queem.Core.Extensions;
using DebutsLib;

namespace Queem.AI
{
	public class ChessSolver
	{
		protected DebutGraph debutGraph;
        protected GameProvider gameProvider;
        protected Move bestMove;
        protected int ply;
        protected int nodesSearched;
	
		public ChessSolver (DebutGraph graph)
		{
			this.debutGraph = graph;
		}

        public int NodesSearched
        {
            get { return this.nodesSearched; }
        }
		
		protected bool TryFindDebutMove(out Move debutMove)
		{
			debutMove = null;
            return false;
		}		
		
		public Move SolveProblem(GameProvider provider, Color color, int maxdepth)
		{
			Move bestMove;
			
			if (this.TryFindDebutMove(out bestMove))
				return bestMove;

            this.gameProvider = provider;
            this.ply = 0;
            this.nodesSearched = 0;
            this.AlphaBetaMain(maxdepth, color);
            return this.bestMove;
		}

        protected int AlphaBetaMain(int depth, Color color)
        {
            ChessTreeNode node = new ChessTreeNode() 
            { 
                Alpha = -Evaluator.MateValue,
                Beta = Evaluator.MateValue,
                Depth = depth,
                PlayerIndex = (int)color
            };

            nodesSearched += 1;

            bool bSearchPV = true;

            var player = this.gameProvider.PlayerBoards[node.PlayerIndex];
            var opponent = this.gameProvider.PlayerBoards[1 - node.PlayerIndex];

            bool wasKingInCheck = player.IsUnderAttack(player.King.GetSquare(), opponent);
            Color currPlayerColor = player.FigureColor;

            var movesArray = player.GetMoves(
                opponent, 
                this.gameProvider.History.GetLastMove(), 
                MovesMask.AllMoves);
            this.gameProvider.FilterMoves(movesArray, currPlayerColor);
            MovesSorter.Sort(movesArray);

            if (movesArray.Size == 0)
            {
                MovesArray.ReleaseLast();
                if (wasKingInCheck)
                    return (-Evaluator.MateValue + ply);
                else
                    return 0;
            }

            int score = -Evaluator.MateValue;

            bool needsPromotion;
            var moves = movesArray.InnerArray;
            for (int i = 0; i < movesArray.Size; ++i)
            {
                var move = moves[i];

                this.gameProvider.ProcessMove(move, player.FigureColor);
                ++ply;

                needsPromotion = (int)move.Type >= (int)MoveType.Promotion;
                if (needsPromotion)
                    this.gameProvider.PromotePawn(
                        currPlayerColor, 
                        move.To, 
                        move.Type.GetPromotionFigure());

                if (bSearchPV)
                    score = -pvSearch(node.GetNext());
                else
                {
                    score = -zwSearch(node.GetNext());
                    if (score > node.Alpha)
                        score = -pvSearch(node.GetNext());
                }

                this.gameProvider.CancelLastMove(currPlayerColor);
                --ply;

                if (score > node.Alpha)
                {
                    node.Alpha = score;
                    this.bestMove = new Move(move);
                    bSearchPV = false;
                }
            }

            MovesArray.ReleaseLast();
            return node.Alpha;
        }

        protected int pvSearch(ChessTreeNode node)
        {
            nodesSearched += 1;

            if (node.IsZeroDepth())
                return this.Quiescence(node);

            bool bSearchPV = true;

            var player = this.gameProvider.PlayerBoards[node.PlayerIndex];
            var opponent = this.gameProvider.PlayerBoards[1 - node.PlayerIndex];

            bool wasKingInCheck = player.IsUnderAttack(player.King.GetSquare(), opponent);
            Color currPlayerColor = player.FigureColor;

            var movesArray = player.GetMoves(
                opponent, 
                this.gameProvider.History.GetLastMove(), 
                MovesMask.AllMoves);
            this.gameProvider.FilterMoves(movesArray, currPlayerColor);
            MovesSorter.Sort(movesArray);

            if (movesArray.Size == 0)
            {
                MovesArray.ReleaseLast();
                if (wasKingInCheck)
                    return (-Evaluator.MateValue + ply);
                else
                    return 0;
            }

            int score = -Evaluator.MateValue;

            bool needsPromotion;
            var moves = movesArray.InnerArray;
            for (int i = 0; i < movesArray.Size; ++i)
            {
                var move = moves[i];

                this.gameProvider.ProcessMove(move, player.FigureColor);
                ++ply;

                needsPromotion = (int)move.Type >= (int)MoveType.Promotion;
                if (needsPromotion)
                    this.gameProvider.PromotePawn(
                        currPlayerColor, 
                        move.To, 
                        move.Type.GetPromotionFigure());

                if (bSearchPV)
                    score = -pvSearch(node.GetNext());
                else
                {
                    score = -zwSearch(node.GetNext());
                    if (score > node.Alpha)
                        score = -pvSearch(node.GetNext());
                }

                this.gameProvider.CancelLastMove(currPlayerColor);
                --ply;

                if (score >= node.Beta)
                {
                    MovesArray.ReleaseLast();
                    return node.Beta;
                }

                if (score > node.Alpha)
                {
                    node.Alpha = score;
                    bSearchPV = false;
                }
            }

            MovesArray.ReleaseLast();
            return node.Alpha;
        }

        protected int zwSearch(ChessTreeNode node)
        {
            if (node.IsZeroDepth())
                return this.Quiescence(node.GetNextQuiescenceZW());

            nodesSearched += 1;

            var player = this.gameProvider.PlayerBoards[node.PlayerIndex];
            var opponent = this.gameProvider.PlayerBoards[1 - node.PlayerIndex];

            bool wasKingInCheck = player.IsUnderAttack(player.King.GetSquare(), opponent);
            Color currPlayerColor = player.FigureColor;

            var movesArray = player.GetMoves(
                opponent, 
                this.gameProvider.History.GetLastMove(), 
                MovesMask.AllMoves);
            this.gameProvider.FilterMoves(movesArray, currPlayerColor);
            //MovesSorter.Sort(movesArray);

            if (movesArray.Size == 0)
            {
                MovesArray.ReleaseLast();
                if (wasKingInCheck)
                    return (-Evaluator.MateValue + ply);
                else
                    return 0;
            }

            int score = -Evaluator.MateValue;

            bool needsPromotion;
            var moves = movesArray.InnerArray;
            for (int i = 0; i < movesArray.Size; ++i)
            {
                var move = moves[i];

                this.gameProvider.ProcessMove(move, player.FigureColor);
                ++ply;

                needsPromotion = (int)move.Type >= (int)MoveType.Promotion;
                if (needsPromotion)
                    this.gameProvider.PromotePawn(
                        currPlayerColor, 
                        move.To, 
                        move.Type.GetPromotionFigure());
                                
                score = -zwSearch(node.GetNextZW());
                
                this.gameProvider.CancelLastMove(currPlayerColor);
                --ply;

                if (score >= node.Beta)
                {
                    MovesArray.ReleaseLast();
                    return node.Beta;
                }
            }

            MovesArray.ReleaseLast();
            return node.Beta - 1;
        }

        protected int Quiescence(ChessTreeNode node)
        {
            nodesSearched += 1;

            var player = this.gameProvider.PlayerBoards[node.PlayerIndex];
            var opponent = this.gameProvider.PlayerBoards[1 - node.PlayerIndex];
            
            int positionValue = Evaluator.Evaluate(player, opponent);

            if (node.Depth < -5)
                return positionValue;
            
            if (positionValue >= node.Beta)
                return node.Beta;

            if (node.Alpha < positionValue)
                node.Alpha = positionValue;
                        
            int score = -Evaluator.MateValue;
            Color currPlayerColor = player.FigureColor;
            bool wasKingInCheck = player.IsUnderAttack(player.King.GetSquare(), opponent);
            
            var movesArray = player.GetMoves(
                            opponent,
                            this.gameProvider.History.GetLastMove(),
                            MovesMask.Attacks);
            this.gameProvider.FilterMoves(movesArray, currPlayerColor);

            if (movesArray.Size == 0)
                if (wasKingInCheck)
                {
                    MovesArray.ReleaseLast();
                    return (-Evaluator.MateValue) + ply;
                }
            
            var moves = movesArray.InnerArray;
            for (int i = 0; i < movesArray.Size; ++i)
            {
                var move = moves[i];

                this.gameProvider.ProcessMove(move, currPlayerColor);
                ply++;

                bool needsPromotion = (int)move.Type >= (int)MoveType.Promotion;
                if (needsPromotion)
                    this.gameProvider.PromotePawn(
                        currPlayerColor, 
                        move.To, 
                        move.Type.GetPromotionFigure());

                score = -Quiescence(node.GetNext());

                this.gameProvider.CancelLastMove(currPlayerColor);
                ply--;

                if (score >= node.Beta)
                {
                    MovesArray.ReleaseLast();
                    return node.Beta;
                }

                if (score > node.Alpha)
                    node.Alpha = score;
            }

            MovesArray.ReleaseLast();
            return node.Alpha;
        }
	}
}

