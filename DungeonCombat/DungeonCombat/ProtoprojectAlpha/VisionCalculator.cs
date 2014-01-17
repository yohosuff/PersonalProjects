using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCombat
{
    class VisionCalculator
    {
        static bool[,] visibility = null;
        static float maximumRayCastDistance = 0;
        static CombatManager battleManager = null;
        static float combatantX = 0;
        static float combatantY = 0;
        static float nextVerticalIntersectionX = 0;
        static float nextVerticalIntersectionY = 0;
        static float nextHorizontalIntersectionX = 0;
        static float nextHorizontalIntersectionY = 0;
        static float verticalIntersectionDeltaX = 0;
        static float verticalIntersectionDeltaY = 0;
        static float horizontalIntersectionDeltaX = 0;
        static float horizontalIntersectionDeltaY = 0;
        static int column = 0;
        static int row = 0;
        static bool loop = false;

        public static void Initialize(CombatManager battleManager, float maxRayCastDistance)
        {
            VisionCalculator.battleManager = battleManager;
            VisionCalculator.maximumRayCastDistance = maxRayCastDistance;
        }

        public static bool[,] GetVisionArray()
        {
            
            visibility = new bool[battleManager.dungeon.dungeonFloor.GetLength(0), battleManager.dungeon.dungeonFloor.GetLength(1)];
            visibility[battleManager.GetCurrentCombatant().location.row, battleManager.GetCurrentCombatant().location.column] = true;
            combatantX = battleManager.GetCurrentCombatant().location.column * Tile.size + Tile.size / 2;
            combatantY = battleManager.GetCurrentCombatant().location.row * Tile.size + Tile.size / 2;

            for (float omega = 0; omega < 360; omega += 1.0f)
            {
                loop = true;

                if (omega > 0 && omega < 90) RayCastQuadrant1(omega);
                else if (omega > 90 && omega < 180) RayCastQuadrant2(180 - omega);
                else if (omega > 180 && omega < 270) RayCastQuadrant3(omega - 180);
                else if (omega > 270 && omega < 360) RayCastQuadrant4(360 - omega);
            }

            return visibility;
        }

        public static void DisplayVisibility()
        {
            for (int row = 0; row < visibility.GetLength(0); row++)
            {
                for (int column = 0; column < visibility.GetLength(1); column++)
                {
                    Console.Write((visibility[row, column]) ? "-" : "X");
                }
                Console.WriteLine();
            }
        }

        public static float DistanceBetweenTwoPoints(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        public static float DegreesToRadians(float degrees)
        {
            return degrees * ((float)Math.PI / 180.0f);
        }

        private static int PixelsToIndex(float pixels)
        {
            return (int)(pixels / (float)Tile.size);
        }

        public static bool IndexInBounds(int row, int column, bool[,] array)
        {
            return
                column >= 0 &&
                column < array.GetLength(1) &&
                row >= 0 &&
                row < array.GetLength(0);
        }

        public static bool IndexInBounds(int row, int column, Tile[,] array)
        {
            return
                column >= 0 &&
                column < array.GetLength(1) &&
                row >= 0 &&
                row < array.GetLength(0);
        }

        private static void CheckVerticalIntersection()
        {
            column = PixelsToIndex(nextVerticalIntersectionX);
            row = PixelsToIndex(nextVerticalIntersectionY);

            if (IndexInBounds(row, column, visibility) &&
                DistanceBetweenTwoPoints(combatantX, combatantY, nextVerticalIntersectionX, nextVerticalIntersectionY) <= maximumRayCastDistance)
            {
                visibility[row, column] = true;

                if (battleManager.dungeon.dungeonFloor[row, column].tileType== 0)
                {
                    nextVerticalIntersectionX += verticalIntersectionDeltaX;
                    nextVerticalIntersectionY += verticalIntersectionDeltaY;
                }
                else
                {
                    loop = false;
                }
            }
            else
            {
                loop = false;
            }
        }

        private static void CheckHorizontalIntersection()
        {
            column = PixelsToIndex(nextHorizontalIntersectionX);
            row = PixelsToIndex(nextHorizontalIntersectionY);

            if (IndexInBounds(row, column, visibility) &&
                maximumRayCastDistance >= DistanceBetweenTwoPoints(combatantX, combatantY, nextHorizontalIntersectionX, nextHorizontalIntersectionY))
            {
                visibility[row, column] = true;

                if (battleManager.dungeon.dungeonFloor[row, column].tileType== 0)
                {
                    nextHorizontalIntersectionX += horizontalIntersectionDeltaX;
                    nextHorizontalIntersectionY += horizontalIntersectionDeltaY;
                }
                else
                {
                    loop = false;
                }
            }
            else
            {
                loop = false;
            }
        }

        public static void RayCastQuadrant1(float omega)
        {
            nextVerticalIntersectionX = (float)Math.Floor(combatantX / Tile.size) * Tile.size + Tile.size;
            nextVerticalIntersectionY = combatantY - (nextVerticalIntersectionX - combatantX) * (float)Math.Tan(DegreesToRadians(omega));
            nextHorizontalIntersectionY = (float)Math.Floor(combatantY / Tile.size) * Tile.size - 1;
            nextHorizontalIntersectionX = combatantX + (combatantY - nextHorizontalIntersectionY) / (float)Math.Tan(DegreesToRadians(omega));

            verticalIntersectionDeltaX = Tile.size;
            verticalIntersectionDeltaY = -Tile.size * (float)Math.Tan(DegreesToRadians(omega));
            horizontalIntersectionDeltaX = Tile.size / (float)Math.Tan(DegreesToRadians(omega));
            horizontalIntersectionDeltaY = -Tile.size;

            while (loop)
            {
                if (DistanceBetweenTwoPoints(combatantX, combatantY, nextVerticalIntersectionX, nextVerticalIntersectionY) < DistanceBetweenTwoPoints(combatantX, combatantY, nextHorizontalIntersectionX, nextHorizontalIntersectionY))
                {
                    if (nextVerticalIntersectionY <= 0) return;
                    CheckVerticalIntersection();
                }
                else //process next horizontal intersection
                {
                    if (nextHorizontalIntersectionY <= 0) return;
                    CheckHorizontalIntersection();
                }
            }
        }

        public static void RayCastQuadrant2(float omega)
        {
            nextVerticalIntersectionX = (float)Math.Floor(combatantX / 64.0f) * 64.0f - 1;
            nextVerticalIntersectionY = combatantY - (combatantX - nextVerticalIntersectionX) * (float)Math.Tan(DegreesToRadians(omega));
            nextHorizontalIntersectionY = (float)Math.Floor(combatantY / 64.0f) * 64.0f - 1;
            nextHorizontalIntersectionX = combatantX - (combatantY - nextHorizontalIntersectionY) / (float)Math.Tan(DegreesToRadians(omega));

            verticalIntersectionDeltaX = -64.0f;
            verticalIntersectionDeltaY = -64.0f * (float)Math.Tan(DegreesToRadians(omega));
            horizontalIntersectionDeltaX = -64.0f / (float)Math.Tan(DegreesToRadians(omega));
            horizontalIntersectionDeltaY = -64.0f;

            while (loop)
            {
                if (DistanceBetweenTwoPoints(combatantX, combatantY, nextVerticalIntersectionX, nextVerticalIntersectionY) < DistanceBetweenTwoPoints(combatantX, combatantY, nextHorizontalIntersectionX, nextHorizontalIntersectionY))
                {
                    if (nextVerticalIntersectionX <= 0 || nextVerticalIntersectionY <= 0) return;
                    CheckVerticalIntersection();
                }
                else //process next horizontal intersection
                {
                    if (nextHorizontalIntersectionX <= 0 || nextHorizontalIntersectionY <= 0) return;
                    CheckHorizontalIntersection();
                }
            }
        }

        public static void RayCastQuadrant3(float omega)
        {
            
            nextVerticalIntersectionX = (float)Math.Floor(combatantX / 64.0f) * 64.0f - 1;
            nextVerticalIntersectionY = combatantY + (combatantX - nextVerticalIntersectionX) * (float)Math.Tan(DegreesToRadians(omega));
            nextHorizontalIntersectionY = (float)Math.Floor(combatantY / 64.0f) * 64.0f + 64.0f;
            nextHorizontalIntersectionX = combatantX - (nextHorizontalIntersectionY - combatantY) / (float)Math.Tan(DegreesToRadians(omega));

            verticalIntersectionDeltaX = -64.0f;
            verticalIntersectionDeltaY = 64.0f * (float)Math.Tan(DegreesToRadians(omega));
            horizontalIntersectionDeltaX = -64.0f / (float)Math.Tan(DegreesToRadians(omega));
            horizontalIntersectionDeltaY = 64.0f;

            while (loop)
            {
                if (DistanceBetweenTwoPoints(combatantX, combatantY, nextVerticalIntersectionX, nextVerticalIntersectionY) < DistanceBetweenTwoPoints(combatantX, combatantY, nextHorizontalIntersectionX, nextHorizontalIntersectionY))
                {
                    if (nextVerticalIntersectionX <= 0) return;
                    CheckVerticalIntersection();
                }
                else //process next horizontal intersection
                {
                    if (nextHorizontalIntersectionX <= 0) return;
                    CheckHorizontalIntersection();
                }
            }
        }

        public static void RayCastQuadrant4(float omega)
        {
            nextVerticalIntersectionX = (float)Math.Floor(combatantX / 64.0f) * 64.0f + 64.0f;
            nextVerticalIntersectionY = combatantY + (nextVerticalIntersectionX - combatantX) * (float)Math.Tan(DegreesToRadians(omega));
            nextHorizontalIntersectionY = (float)Math.Floor(combatantY / 64.0f) * 64.0f + 64.0f;
            nextHorizontalIntersectionX = combatantX + (nextHorizontalIntersectionY - combatantY) / (float)Math.Tan(DegreesToRadians(omega));

            verticalIntersectionDeltaX = 64.0f;
            verticalIntersectionDeltaY = 64.0f * (float)Math.Tan(DegreesToRadians(omega));
            horizontalIntersectionDeltaX = 64.0f / (float)Math.Tan(DegreesToRadians(omega));
            horizontalIntersectionDeltaY = 64.0f;

            while (loop)
            {
                if (DistanceBetweenTwoPoints(combatantX, combatantY, nextVerticalIntersectionX, nextVerticalIntersectionY) < DistanceBetweenTwoPoints(combatantX, combatantY, nextHorizontalIntersectionX, nextHorizontalIntersectionY))
                {
                    CheckVerticalIntersection();
                }
                else //process next horizontal intersection
                {
                    CheckHorizontalIntersection();
                }
            }
        }
    }
}
