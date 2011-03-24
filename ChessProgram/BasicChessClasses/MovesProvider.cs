using System;
using System.Collections.Generic;

namespace BasicChessClasses
{
    public enum MoveResult
    {
        MoveOk, Castling, CapturedInPassing,
        CaptureOk, Fail, CapturedAndPawnReachedEnd, PawnReachedEnd
    };

    public partial class MovesProvider
    {
        #region Data

        protected ChessPlayerBase player1;
        protected ChessPlayerBase player2;

        // must not use board[int, int] in this file
        // only in ChessBoard class
        protected ChessBoard board;
        protected MovesHistory history;

        // when copying this members, just copy a link
        // deep copy is not needed
        protected FigureMapper figureMapper;
        protected CellControlFiguresMap controlMapper;

        // move processors
        protected PawnProcessor pawnProcessor1;
        protected PawnProcessor pawnProcessor2;
        protected KingProcessor kingProcessor;
        protected HorseProcessor horseProcessor;

        protected SortedDictionary<ulong, int> positionCounts;

        // position of white figures on board
        protected FigureStartPosition whitePos;

        //protected delegate List<Coordinates> GetSomebodyMovesDelegate (Coordinates coords);
        // todo make some stupid delegate for Nobody and Null figure types
        //protected Dictionary<FigureType, GetSomebodyMovesDelegate> movesGenerators;

        #endregion

        public MovesProvider(FigureColor player1Color, FigureStartPosition player1StartPos)
        {
            player1 = new ChessPlayerBase(player1Color, player1StartPos);

            FigureColor player2Color = player1.FiguresColor.GetOppositeColor();

            FigureStartPosition player2StartPos = player1.StartPos.GetOppositePosition();

            player2 = new ChessPlayerBase(player2Color, player2StartPos);

            board = new ChessBoard(player1StartPos, player1Color);
            history = new MovesHistory();

            figureMapper = new FigureMapper();
            controlMapper = new CellControlFiguresMap();

            // TODO add code for hashing positions 
            // after provide move and cancel move 
            positionCounts = new SortedDictionary<ulong, int>();
            positionCounts.Add(board.HashCode, 1);

            #region Processors

            pawnProcessor1 = new PawnProcessor(player1StartPos);
            pawnProcessor2 = new PawnProcessor(player2StartPos);

            // king processor doesn't depend on start position
            // that's why it would be just one
            kingProcessor = new KingProcessor();

            // horse processor also doesn't depend on 
            // start position that's why it would be just one
            horseProcessor = new HorseProcessor();

            #endregion

            #region Delegates
            /*
			// ------ delegates --------
			movesGenerators = new Dictionary<FigureType, GetSomebodyMovesDelegate> ();
			movesGenerators.Add (FigureType.Pawn, this.GetPawnMoves);
			movesGenerators.Add (FigureType.King, this.GetKingMoves);
			movesGenerators.Add (FigureType.Horse, this.GetHorseMoves);
			movesGenerators.Add (FigureType.Rook, this.GetRookMoves);
			movesGenerators.Add (FigureType.Bishop, this.GetBishopMoves);
			movesGenerators.Add (FigureType.Queen, this.GetQueenMoves);
			
			movesGenerators.Add (FigureType.Nobody, delegate(Coordinates coords)
			                     {
				return new List<Coordinates> ();
			});
			movesGenerators.Add (FigureType.Null, delegate(Coordinates coords)
			                     {
				return new List<Coordinates> ();
			});
			*/
            #endregion
        }

        protected MovesProvider(MovesProvider copy)
        {
            player1 = (ChessPlayerBase)copy.player1.Clone();
            player2 = (ChessPlayerBase)copy.player2.Clone();

            board = (ChessBoard)copy.board.Clone();

            positionCounts = new SortedDictionary<ulong, int>(copy.positionCounts);
            whitePos = copy.whitePos;

            // i wont need to cancel moves in this clone
            // but for debug make it
#if DEBUG
            history = new MovesHistory(history);
#else
			history = new MovesHistory ();
#endif
            // and other not deep copying

            figureMapper = copy.figureMapper;
            controlMapper = copy.controlMapper;

            pawnProcessor1 = copy.pawnProcessor1;
            pawnProcessor2 = copy.pawnProcessor2;

            kingProcessor = copy.kingProcessor;
            horseProcessor = copy.horseProcessor;
        }

        public virtual MoveResult ProvidePlayerMove(ChessMove move,
                                             ChessPlayerBase player, ChessPlayerBase opponentPlayer)
        {
            /*
             * Possible problematic situations
             * 
             * ----------Pawn-----------
             *  1. move to in-passing state
             *  2. move from in-passing state
             *  3. kill other pawn that is in passing state
             *  4. just move
             *  5. kill a rook in state with possible castling
             * 
             * 
             * ----------Rook----------
             *  1. kill a pawn in a passing state
             *  2. move from castling state
             *  3. be a part of a castling
             *  4. kill other rook in a castling state
             *  5. just move
             * 
             * 
             * ----------King----------
             *  1. move from a castling state
             *  2. kill a pawn in a passing state
             *  3. kill a rook in a castling state
            */

            // get links for FigureManager classes of players
            FiguresManager myManager = player.FiguresManager;
            FiguresManager opponentManager = opponentPlayer.FiguresManager;

            // on this stage we assume, that ChessMove move
            // is a correct chess move for figure, that is
            // on move.Start coordinate

            GeneralFigure figureMoving = board[move.Start];
            GeneralFigure destinationFigure = board[move.End];

            DeltaChanges changes = new DeltaChanges();
            Change moveChange = new Change(MoveAction.Move, move, figureMoving.Type, figureMoving.Color);

            int movesCount = history.Moves.Count;

            MoveResult moveResult = MoveResult.Fail;

            moveResult = MoveResult.MoveOk;

            #region Killing a figure

            // a killing move except a pawn in passing state
            if (destinationFigure.Type != FigureType.Nobody)
            {
                // in this section we are capturing
                // some opponents' figure

                Change killChange = new Change(MoveAction.Deletion,
                                                move.End,
                                                destinationFigure.Type,
                                                destinationFigure.Color);

                killChange.Data =
                    opponentManager.GetBoolPropertyOfFigure(destinationFigure.Type, move.End);

                // at first remove opponents' figure
                changes.Add(killChange);
                opponentManager.RemoveFigure(destinationFigure.Type, move.End);

                moveResult = MoveResult.CaptureOk;

                // i don't like this 2 IFs here
                // but seems to be, that i've no choise

                // if a pawn reaches end with killing other figure
                if (figureMoving.Type == FigureType.Pawn)
                    if ((move.End.Y == 0) || (move.End.Y == 7))
                        moveResult = MoveResult.CapturedAndPawnReachedEnd;

                // now provide move itself
                moveChange.Data = myManager.GetBoolPropertyOfFigure(figureMoving.Type, move.Start);
                // now add changes
                changes.Add(moveChange);
                myManager.MoveFigure(figureMoving.Type, move.Start, move.End);

                board.ProvideMove(move, moveResult);

                history.Add(move, moveResult, changes);
                return moveResult;
            }

            #endregion

            // now process pawn move
            // first check if it was a "check in passing move"

            ChessMove lastMove = new ChessMove();
            if (movesCount > 0)
                lastMove = history.Moves[movesCount - 1];

            #region Pawn logic

            // first special case - can be pawn captured in passing state
            // in this case all pawn possible moves are
            if (figureMoving.Type == FigureType.Pawn)
            {
                // if last move was a pawn move to in-passing state

                // if last figure, that moved was a pawn
                if ((movesCount > 0) && board[lastMove.End].Type == FigureType.Pawn)
                {
                    Pawn lastPawn = opponentManager.Pawns[lastMove.End];

                    // if this pawn is moving to that pawn
                    if (lastPawn.Coordinates.Letter == move.End.Letter)
                    {
                        if (lastPawn.Coordinates.Y == move.Start.Y)
                        {
                            // now it is an in-passing capture state

                            Change inPassingCaptureChange =
                                new Change(MoveAction.Deletion,
                                           lastPawn.Coordinates,
                                           FigureType.Pawn,
                                           lastPawn.Color);

                            changes.Add(inPassingCaptureChange);
                            opponentManager.RemoveFigure(FigureType.Pawn, lastPawn.Coordinates);

                            changes.Add(moveChange);
                            myManager.MoveFigure(FigureType.Pawn, move.Start, move.End);

                            moveResult = MoveResult.CapturedInPassing;

                            board.ProvideMove(move, moveResult);

                            history.Add(move, moveResult, changes);
                            return moveResult;
                        }
                    }
                }

                // else it is just a simple move
                
                changes.Add(moveChange);
                myManager.MoveFigure(FigureType.Pawn, move.Start, move.End);
                
                if ((move.End.Y == 0) || (move.End.Y == 7))
                    moveResult = MoveResult.PawnReachedEnd;
                else
                    moveResult = MoveResult.MoveOk;

                board.ProvideMove(move, moveResult);

                history.Add(move, moveResult, changes);
                return moveResult;
            }

            #endregion

            if (figureMoving.Type == FigureType.Rook)
            {
                Rook rookMoving = myManager.Rooks[move.Start];
                moveChange.Data = rookMoving.CanDoCastling;
                rookMoving.CanDoCastling = false;
            }

            #region King castling

            // second special case - king can make castling
            if (figureMoving.Type == FigureType.King)
            {
                if (Math.Abs(move.Start.X - move.End.X) == 2)
                {
                    int difference = move.End.X - move.Start.X;
                    ChessMove rookMove = new ChessMove(move);
                    rookMove.End.Letter = move.End.Letter - Math.Sign(difference);

                    if (difference > 0)
                    {
                        // short castling
                        rookMove.Start.Letter = FieldLetter.H;
                    }
                    else
                    {
                        // long castling
                        rookMove.Start.Letter = FieldLetter.A;
                    }

                    Change rookMoveChange = new Change(MoveAction.Move, rookMove,
                                                       FigureType.Rook, player.FiguresColor);
                    // rook was able to do castling
                    rookMoveChange.Data = true;

                    // king was able to do castling
                    moveChange.Data = true;
                    // abandon king to do castling again
                    myManager.Kings.King.CanDoCastling = false;

                    // first is king move
                    changes.Add(moveChange);
                    myManager.MoveFigure(FigureType.King, move.Start, move.End);

                    // than add a rook move
                    changes.Add(rookMoveChange);
                    myManager.MoveFigure(FigureType.Rook, rookMove.Start, rookMove.End);

                    moveResult = MoveResult.Castling;

                    board.ProvideMove(move, moveResult);

                    // FIX: forgot to make rook move on board
                    // need to check in future if this line is
                    // in right place!
                    board.ProvideMove(rookMove, MoveResult.MoveOk);

                    history.Add(move, moveResult, changes);
                    return moveResult;
                }

                // else now 
                King king = myManager.Kings[move.Start];
                moveChange.Data = king.CanDoCastling;
                king.CanDoCastling = false;
            }

            #endregion

            // now add changes
            changes.Add(moveChange);
            myManager.MoveFigure(figureMoving.Type, move.Start, move.End);

            board.ProvideMove(move, moveResult);

            history.Add(move, moveResult, changes);
            return moveResult;
        }

        public virtual void CancelLastPlayerMove(ChessPlayerBase player,
                                                     ChessPlayerBase opponentPlayer)
        {
            // get links for FigureManager classes of players
            FiguresManager myManager = player.FiguresManager;
            FiguresManager opponentManager = opponentPlayer.FiguresManager;

            ChessMove move = null;

            DeltaChanges lastChanges = history.LastChanges;

            while (lastChanges.Changes.Count > 0)
            {
                Change change = lastChanges.Changes.Pop();

                switch (change.Action)
                {
                    case MoveAction.PawnChange:
                        myManager.RestoreFigure(change.FigureType, change.Coords);

                        myManager.SetBoolPropertyOfFigure(change.FigureType,
                                                                  change.Coords, change.Data);
                        // cancel this change on board
                        board.CancelDeletion(change);
                        break;
                    case MoveAction.Deletion:

                        opponentManager.RestoreFigure(change.FigureType, change.Coords);

                        opponentManager.SetBoolPropertyOfFigure(change.FigureType,
                                                                  change.Coords, change.Data);
                        // cancel this change on board
                        board.CancelDeletion(change);

                        break;

                    case MoveAction.Move:

                        move = new ChessMove();

                        // get this move
                        move.Start = change.Coords;
                        move.End = change.AdditionalCoords;

                        // move figure backward
                        myManager.MoveFigure(change.FigureType, move.End, move.Start);

                        myManager.SetBoolPropertyOfFigure(change.FigureType,
                                                            change.Coords, change.Data);

                        // cancel this change on board
                        board.CancelMove(change);

                        break;

                    case MoveAction.Creation:

                        // i wont loose my hashes here
                        myManager.DestroyFigure(change.FigureType, change.Coords);

                        // cancel this change on board
                        board.CancelCreation(change);

                        break;

                    default:
                        break;
                }
            }

            history.UndoLast();
        }

        public virtual void ReplacePawnAtTheOtherSide(Coordinates pawnCoords,
                                                       FigureType newType, ChessPlayerBase player)
        {
            Change deleteChange =
                new Change(MoveAction.PawnChange, pawnCoords, FigureType.Pawn, player.FiguresColor);
            Change createChange =
                new Change(MoveAction.Creation, pawnCoords, newType, player.FiguresColor);

            // add changes for history
            DeltaChanges changes = history.DeltaChanges[history.DeltaChanges.Count - 1];

            changes.Add(deleteChange);
            player.FiguresManager.RemoveFigure(FigureType.Pawn, pawnCoords);

            changes.Add(createChange);
            player.FiguresManager.AddFigure(pawnCoords, newType, player.FiguresColor);

            board[pawnCoords].Type = newType;
        }

        protected virtual bool IsUnderPlayerControl(int coordsX, int coordsY, ChessPlayerBase player)
        {
            // initial coordinates
            int initX = coordsX;
            int initY = coordsY;

            // current coordinates
            int x = initX;
            int y = initY;

            /*
             * First check for pawn.
             * For that find direction, where
             * opponent pawns can be
             * (indexation of "y" coordinate grows 
             *  from up(0) to down(7))
            */

            // first check diagonal (for pawns)
            int direction = 1;

            if (player.StartPos == FigureStartPosition.Up)
                direction *= -1;

            #region Pawns check

            GeneralFigure gf = board[x - 1, y + direction];

            if (gf.Type == FigureType.Pawn)
                if (gf.Color == player.FiguresColor)
                    return true;

            gf = board[x + 1, y + direction];

            if (gf.Type == FigureType.Pawn)
                if (gf.Color == player.FiguresColor)
                    return true;

            #endregion

            #region Diagonals check

            if ((player.FiguresManager.Queens.Count > 0) ||
                (player.FiguresManager.Bishops.Count > 0))
            {
                int x1 = initX - 1;
                int x2 = initX + 1;
                y = initY - 1;

                while (y >= 0)
                {
                    #region Left diagonal check

                    if (x1 >= 0)
                    {
                        if (board[(FieldLetter)x1, y].Type != FigureType.Nobody)
                        {
                            GeneralFigure figure = board[(FieldLetter)x1, y];

                            if (controlMapper.GetDiagonalsMap(player.FiguresColor)[figure] == 1)
                                return true;

                            // finish this if loop
                            x1 = 0;

                            if (x2 > 7)
                                break;
                        }

                        --x1;
                    }

                    #endregion

                    #region Right diagonal check

                    if (x2 <= 7)
                    {
                        if (board[(FieldLetter)x2, y].Type != FigureType.Nobody)
                        {
                            GeneralFigure figure = board[(FieldLetter)x2, y];

                            if (controlMapper.GetDiagonalsMap(player.FiguresColor)[figure] == 1)
                                return true;

                            // finish this if loop
                            x2 = 7;

                            if (x1 < 0)
                                break;
                        }

                        ++x2;
                    }

                    #endregion

                    --y;
                }

                x1 = initX - 1;
                x2 = initX + 1;
                y = initY + 1;

                while (y <= 7)
                {
                    #region Left diagonal check

                    if (x1 >= 0)
                    {
                        if (board[(FieldLetter)x1, y].Type != FigureType.Nobody)
                        {
                            GeneralFigure figure = board[(FieldLetter)x1, y];

                            if (controlMapper.GetDiagonalsMap(player.FiguresColor)[figure] == 1)
                                return true;

                            // finish this if loop
                            x1 = 0;
                            if (x2 > 7)
                                break;
                        }

                        --x1;
                    }

                    #endregion

                    #region Right diagonal check

                    if (x2 <= 7)
                    {
                        if (board[(FieldLetter)x2, y].Type != FigureType.Nobody)
                        {
                            GeneralFigure figure = board[(FieldLetter)x2, y];

                            if (controlMapper.GetDiagonalsMap(player.FiguresColor)[figure] == 1)
                                return true;

                            // finish this if loop
                            x2 = 7;

                            if (x1 < 0)
                                break;
                        }

                        ++x2;
                    }

                    #endregion

                    ++y;
                }
            }

            #endregion

            #region Verticals and Horisontals check

            if ((player.FiguresManager.Queens.Count > 0) ||
                (player.FiguresManager.Rooks.Count > 0))
            {
                // horisontals check

                y = initY;

                x = initX - 1;
                while (x >= 0)
                {
                    if (board[(FieldLetter)x, y].Type != FigureType.Nobody)
                    {
                        GeneralFigure figure = board[(FieldLetter)x, y];

                        if (controlMapper.GetVHMap(player.FiguresColor)[figure] == 1)
                            return true;

                        break;
                    }
                    --x;
                }

                x = initX + 1;
                while (x <= 7)
                {
                    if (board[(FieldLetter)x, y].Type != FigureType.Nobody)
                    {
                        GeneralFigure figure = board[(FieldLetter)x, y];

                        if (controlMapper.GetVHMap(player.FiguresColor)[figure] == 1)
                            return true;

                        break;
                    }
                    ++x;
                }


                // verticals check

                x = initX;

                y = initY - 1;
                while (y >= 0)
                {
                    if (board[(FieldLetter)x, y].Type != FigureType.Nobody)
                    {
                        GeneralFigure figure = board[(FieldLetter)x, y];

                        if (controlMapper.GetVHMap(player.FiguresColor)[figure] == 1)
                            return true;

                        break;
                    }
                    --y;
                }

                y = initY + 1;
                while (y <= 7)
                {
                    if (board[(FieldLetter)x, y].Type != FigureType.Nobody)
                    {
                        GeneralFigure figure = board[(FieldLetter)x, y];

                        if (controlMapper.GetVHMap(player.FiguresColor)[figure] == 1)
                            return true;

                        break;
                    }
                    ++y;
                }
            }

            #endregion

            #region Horse check

            foreach (Horse horse in player.FiguresManager.Horses)
            {
                Coordinates horseCoords = horse.Coordinates;

                int deltaX = Math.Abs(coordsX - horseCoords.X);
                int deltaY = Math.Abs(coordsY - horseCoords.Y);

                if (((deltaX == 2) && (deltaY == 1)) ||
                    ((deltaX == 1) && (deltaY == 2)))
                    return true;
            }

            /*
				
                Dictionary<GeneralFigure, byte> horseMap = 
                    controlMapper.HorseMap[player.FiguresColor];
                x = initX;
                y = initY;
				
                // upper part
				
                if (horseMap[ board[x - 2, y - 1] ] == 1)
                    return true;
				
                if (horseMap[ board[x - 1, y - 2] ] == 1)
                    return true;
				
                if (horseMap[ board[x + 1, y - 2] ] == 1)
                    return true;
				
                if (horseMap[ board[x + 2, y - 1] ] == 1)
                    return true;
				
                // lower part
				
                if (horseMap[ board[x + 2, y + 1] ] == 1)
                    return true;
				
                if (horseMap[ board[x + 1, y + 2] ] == 1)
                    return true;
				
                if (horseMap[ board[x - 1, y + 2] ] == 1)
                    return true;
				
                if (horseMap[ board[x - 2, y + 1] ] == 1)
                    return true;
                */

            #endregion

            #region King check

            Coordinates kingCoords = player.FiguresManager.Kings.King.Coordinates;

            int delta_X = Math.Abs(kingCoords.X - coordsX);
            int delta_Y = Math.Abs(kingCoords.Y - coordsY);

            if (Math.Max(delta_X, delta_Y) == 1)
                return true;

            #endregion

            return false;
        }

        protected virtual bool IsInCheck(ChessPlayerBase player, ChessPlayerBase opponentPlayer)
        {
            Coordinates coords = player.FiguresManager.Kings.King.Coordinates;
            return IsUnderPlayerControl(coords.X, coords.Y, opponentPlayer);
        }

        protected virtual bool IsUnderAttackFromDirection(Coordinates targetCoords,
                                                            Coordinates fromCoords)
        {
            int x, y, directionX, directionY;
            int criticalY, criticalX;

            FigureColor color = board[targetCoords].Color;

            // if on same vertical
            if (targetCoords.Letter == fromCoords.Letter)
            {
                directionY = Math.Sign(fromCoords.Y - targetCoords.Y);

                y = targetCoords.Y + directionY;

                // if direction == 1, than critical = 7
                // else if direction == -1, than critical = 0
                criticalY = (int)(3.5 + directionY * 3.5) + directionY;

                while (y != criticalY)
                {
                    if (board[fromCoords.Letter, y].Type != FigureType.Nobody)
                    {
                        // if enemy stands there
                        if (board[fromCoords.Letter, y].Color != color)
                        {
                            switch (board[fromCoords.Letter, y].Type)
                            {
                                case FigureType.Rook:
                                case FigureType.Queen:
                                    return true;
                                default:
                                    return false;
                            }
                        }

                        return false;
                    }

                    y += directionY;
                }

                return false;
            }

            // if on same horisontal
            if (targetCoords.Y == fromCoords.Y)
            {
                directionX = Math.Sign(fromCoords.X - targetCoords.X);

                x = targetCoords.X + directionX;

                // if direction == 1, than critical = 7
                // else if direction == -1, than critical = 0
                criticalX = (int)(3.5 + directionX * 3.5) + directionX;

                int tempY = fromCoords.Y;

                while (x != criticalX)
                {
                    if (board[(FieldLetter)x, tempY].Type != FigureType.Nobody)
                    {
                        // if enemy stands there
                        if (board[(FieldLetter)x, tempY].Color != color)
                        {
                            switch (board[(FieldLetter)x, tempY].Type)
                            {
                                case FigureType.Rook:
                                case FigureType.Queen:
                                    return true;
                                default:
                                    return false;
                            }
                        }

                        return false;
                    }

                    x += directionX;
                }

                return false;
            }

            // if on same diagonal
            if (Math.Abs(targetCoords.X - fromCoords.X) == Math.Abs(targetCoords.Y - fromCoords.Y))
            {
                directionY = Math.Sign(fromCoords.Y - targetCoords.Y);

                y = targetCoords.Y + directionY;

                // if direction == 1, than critical = 7
                // else if direction == -1, than critical = 0
                criticalY = (int)(3.5 + directionY * 3.5) + directionY;

                // -------------------------------------

                directionX = Math.Sign(fromCoords.X - targetCoords.X);

                x = targetCoords.X + directionX;

                // if direction == 1, than critical = 7
                // else if direction == -1, than critical = 0
                criticalX = (int)(3.5 + directionX * 3.5) + directionX;

                // ----------------------------------------

                while ((x != criticalX) && (y != criticalY))
                {
                    if (board[(FieldLetter)x, y].Type != FigureType.Nobody)
                    {
                        // if enemy stands there
                        if (board[(FieldLetter)x, y].Color != color)
                        {
                            switch (board[(FieldLetter)x, y].Type)
                            {
                                case FigureType.Bishop:
                                case FigureType.Queen:
                                    return true;
                                default:
                                    return false;
                            }
                        }

                        return false;
                    }

                    x += directionX;
                    y += directionY;
                }


                return false;
            }

            return false;
        }

        protected virtual bool IsInCheckAfterMyMove(ChessMove myMove, ChessPlayerBase player,
                                                 ChessPlayerBase opponentPlayer)
        {
            /*
             * I assume, that before my move 
             * king wasn't in check state
             * (and this move wasn't made by players' king)
             * 
             * now must check if my move have not
             * opened some figure
            */

            // if king moved, than make full cheking
            if (board[myMove.End].Type == FigureType.King)
                return IsInCheck(player, opponentPlayer);

            Coordinates kingCoords = player.FiguresManager.Kings.King.Coordinates;

            Coordinates coords = myMove.Start;

            return IsUnderAttackFromDirection(kingCoords, coords);
        }

        protected virtual bool IsInCheckAfterOpponentMove(ChessMove opponentMove, ChessPlayerBase player,
                                                 ChessPlayerBase opponentPlayer)
        {
            Coordinates coords = player.FiguresManager.Kings.King.Coordinates;

            if (board[opponentMove.End].Type == FigureType.Horse)
            {
                foreach (Horse horse in opponentPlayer.FiguresManager.Horses)
                {
                    Coordinates horseCoords = horse.Coordinates;

                    int deltaX = Math.Abs(coords.X - horseCoords.X);
                    int deltaY = Math.Abs(coords.Y - horseCoords.Y);

                    if (((deltaX == 2) && (deltaY == 1)) ||
                        ((deltaX == 1) && (deltaY == 2)))
                        return true;
                }

                return false;
            }

            if (board[opponentMove.End].Type == FigureType.Pawn)
            {
                int x = coords.X;
                int y = coords.Y;

                // first check diagonal (for pawns)
                int direction = -1;

                if (player.StartPos == FigureStartPosition.Up)
                    direction *= -1;

                GeneralFigure gf = board[x - 1, y + direction];

                if (gf.Type == FigureType.Pawn)
                    if (gf.Color == opponentPlayer.FiguresColor)
                        return true;

                gf = board[x + 1, y + direction];

                if (gf.Type == FigureType.Pawn)
                    if (gf.Color == opponentPlayer.FiguresColor)
                        return true;

                return false;
            }

            if (IsUnderAttackFromDirection(coords, opponentMove.Start))
                return true;

            if (IsUnderAttackFromDirection(coords, opponentMove.End))
                return true;

            return false;
        }

        /// <summary>
        /// Cycles though all possible cells and takes all moves
        /// after which players' king in not in check. Clears list
        /// of input cells
        /// </summary>
        /// <param name="coords"></param>
        /// <param name="possibleCells"></param>
        /// <param name="player"></param>
        /// <param name="opponentPlayer"></param>
        /// <returns>List of coordinates, where figure safely for king can move</returns>
        protected virtual List<Coordinates> FilterMoves(Coordinates coords,
                                                         List<Coordinates> possibleCells,
                                                         ChessPlayerBase player,
                                                         ChessPlayerBase opponentPlayer)
        {
            List<Coordinates> resultCoords =
                new List<Coordinates>(possibleCells.Count);

            ChessMove move = new ChessMove();
            move.Start = coords;

            if ((history.Count > 0) && IsInCheckAfterOpponentMove(history.LastMove, player, opponentPlayer))
            {
                var lastMove = history.LastMove;

                for (int i = 0; i < possibleCells.Count; ++i)
                {
                    move.End = possibleCells[i];

                    // cannot use ProvidePlayerMoveOnBoard because
                    // it uses horse check of real player horses
                    // (and -*-OnBoard wont touch player figures)
                    ProvidePlayerMove(move, player, opponentPlayer);

                    /*
                     * Optimization here can be provided...
                     * We can first check if king moved
                     * if yes, then make full IsInCheck()
                     * else check AttackFromDirections() 
                     * from move.start and move.end
                    */
                    if (!IsInCheck(player, opponentPlayer))
                        resultCoords.Add(possibleCells[i]);
                    /*
                    if (board[move.End].Type == FigureType.King)
                    {
                        if (!IsInCheck(player, opponentPlayer))
                            resultCoords.Add(possibleCells[i]);
                    }
                    else
                    {
                        bool notInCheck = true;
                        Coordinates kingCoords = player.FiguresManager.Kings.King.Coordinates;

                        notInCheck = IsInCheckAfterOpponentMove(lastMove, player, opponentPlayer);

                        if (notInCheck)
                            if (IsUnderAttackFromDirection(kingCoords, move.Start))
                                notInCheck = false;

                        if (notInCheck)
                            if (IsUnderAttackFromDirection(kingCoords, move.End))
                                notInCheck = false;

                        if (notInCheck)
                            resultCoords.Add(possibleCells[i]);
                    }
                    */
                    CancelLastPlayerMove(player, opponentPlayer);
                }
            }
            else
            {
                for (int i = 0; i < possibleCells.Count; ++i)
                {
                    move.End = possibleCells[i];

                    // cannot use ProvidePlayerMoveOnBoard because
                    // it uses horse check of real player horses
                    // (and -*-OnBoard wont touch player figures)
                    ProvidePlayerMove(move, player, opponentPlayer);

                    if (!IsInCheckAfterMyMove(move, player, opponentPlayer))
                        resultCoords.Add(possibleCells[i]);

                    CancelLastPlayerMove(player, opponentPlayer);
                }
            }

            possibleCells.Clear();

            return resultCoords;
        }

        public MovesProvider GetSnapshot()
        {
            return new MovesProvider(this);
        }
    }
}
