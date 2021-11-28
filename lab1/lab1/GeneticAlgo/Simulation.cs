using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticAlgo
{
    public class Simulation
    {
        public Random rand;
        public int PoolSize { get; set; }
        public int GenerationNum { get; set; }
        public double ProbabilityOfCrossover { get; set; }
        public int CrossoverPoints { get; set; }
        public double ProbabilityOfMutation { get; set; }
        public int ElitePercentage { get; set; }
        public Generation gen;
        public string EncryptedText { get; set; }
        public FrequencyCalculation DefTrigramCounts { get; set; }

        const string ENG_ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public Simulation(int poolSize, double crossover, int crossoverPoints, double mutation, int elitePercentage, string encryptedText)
        {
            GenerationNum = 1;
            rand = new Random();

            PoolSize = poolSize;
            ProbabilityOfCrossover = crossover;
            CrossoverPoints = crossoverPoints;
            ProbabilityOfMutation = mutation;
            ElitePercentage = elitePercentage;
            EncryptedText = encryptedText;

            gen = new Generation(EncryptedText);
        }

        public void SimulationStart()
        {
            FrequencyCalculation frequenciesCalculation = new FrequencyCalculation();
            FrequencyCalculation.PopulateDefTrigramCounts();

            Pool pool1 = new Pool(PoolSize, EncryptedText);
            pool1.CreatePool();
            Pool pool2 = new Pool(PoolSize, EncryptedText);
            pool2.CreatePool();

            pool1.EnsureDistinctPool();
            pool2.EnsureDistinctPool();

            gen.CreateFirstGeneration(pool1, pool2);
            gen.EnsureDistinctPools();
            gen.PoolList[0].PossibleKeysList.Sort(ComparatorForSorting);
            gen.PoolList[1].PossibleKeysList.Sort(ComparatorForSorting);
        }

        public List<PKey> Breed(PKey key1, PKey key2)
        {
            var childrens = new List<PKey>();
            var probOfCrossover = rand.NextDouble();
            var keyLength = key1.KeyValue.Length;
            var childKey1 = new char[keyLength]; var childKey2 = new char[keyLength];
            // Crossover
            for (int i = 0; i < keyLength; i++)
            {
                childKey1[i] = key1.KeyValue[i];
                childKey2[i] = key2.KeyValue[i];
            }
            
            // Mutation
            var probOfKey1Mutate = rand.NextDouble();
            var probOfKey2Mutate = rand.NextDouble();
            if (probOfKey1Mutate < ProbabilityOfMutation)
            {
                int oldPosition = rand.Next(0, keyLength);
                int newPosition = -1;
                do
                {
                    newPosition = rand.Next(0, keyLength);
                } while (oldPosition == newPosition);
                char oldChar = childKey1[oldPosition];
                childKey1[oldPosition] = childKey1[newPosition];
                childKey1[newPosition] = oldChar;
            }

            if (probOfKey2Mutate < ProbabilityOfMutation)
            {
                int oldPosition = rand.Next(0, keyLength);
                int newPosition = -1;
                do
                {
                    newPosition = rand.Next(0, keyLength);
                } while (oldPosition == newPosition);
                char oldChar = childKey2[oldPosition];
                childKey2[oldPosition] = childKey2[newPosition];
                childKey2[newPosition] = oldChar;
            }

            childrens.Add(new PKey(new String(childKey1), EncryptedText));
            childrens.Add(new PKey(new String(childKey2), EncryptedText));

            return childrens;
        }

        private int ComparatorForSorting(PKey key1, PKey key2)
        {
            return -key1.FitnessValue.CompareTo(key2.FitnessValue);
        }

        public void SimulationStep()
        {
            int eliteAmount = (int)Math.Ceiling((2 * PoolSize * ((double)ElitePercentage / 100)));
            int childrenToMake = (2 * PoolSize) - eliteAmount;

            var keysOfPool1 = gen.PoolList[0].PossibleKeysList;
            var keysOfPool2 = gen.PoolList[1].PossibleKeysList;

            
            int j = 0, k = 0;
            Generation nextGen = new Generation(EncryptedText);
            nextGen.CreateFirstGeneration(new Pool(PoolSize, EncryptedText), new Pool(PoolSize, EncryptedText));
            if (eliteAmount > 0)
            {
                for (int i = 0; i < eliteAmount; i++)
                {
                    if (i % 2 == 0)
                    {
                        nextGen.PoolList[0].PossibleKeysList.Add(keysOfPool1[j]);
                        j++;
                    }
                    else
                    {
                        nextGen.PoolList[1].PossibleKeysList.Add(keysOfPool2[k]);
                        k++;
                    }
                }
            }

            int elite1 = j;
            int elite2 = k;

            if (childrenToMake > 0)
            {
                var allChildren = new List<PKey>();

                for (int m = 0; m < elite1; m++)
                {
                    for (int n = 0; n < elite2; n++)
                    {
                        allChildren.AddRange(Breed(keysOfPool1[m], keysOfPool2[n]));
                    }
                }

                for (int n = 0; n < elite2; n++)
                {
                    for (int m = 0; m < elite1; m++)
                    {
                        allChildren.AddRange(Breed(keysOfPool1[n], keysOfPool2[m]));
                    }
                }

                for (int i = 0; i < allChildren.Count; i++)
                {
                    if (nextGen.PoolList[0].PossibleKeysList.Count + nextGen.PoolList[1].PossibleKeysList.Count < 2 * PoolSize)
                    {
                        if (i % 2 == 0)
                            nextGen.PoolList[0].PossibleKeysList.Add(allChildren[i]);
                        else
                            nextGen.PoolList[1].PossibleKeysList.Add(allChildren[i]);
                    }
                }

                int moreToAdd = (2 * PoolSize) - (nextGen.PoolList[0].PossibleKeysList.Count + nextGen.PoolList[1].PossibleKeysList.Count);
                allChildren.Clear();

                for (int i = j; i < PoolSize; i++)
                {
                    if (moreToAdd <= allChildren.Count)
                        break;
                    allChildren.AddRange(Breed(keysOfPool1[i], keysOfPool2[i]));
                }
                int counter = 0;
                while (moreToAdd > 0)
                {
                    if (nextGen.PoolList[0].PossibleKeysList.Count > nextGen.PoolList[1].PossibleKeysList.Count)
                    {
                        nextGen.PoolList[1].PossibleKeysList.Add(allChildren[counter]);
                        counter++;
                    }
                    else
                    {
                        nextGen.PoolList[0].PossibleKeysList.Add(allChildren[counter]);
                        counter++;
                    }
                    moreToAdd--;
                }
            }
            nextGen.PoolList[0].EnsureDistinctPool();
            nextGen.PoolList[1].EnsureDistinctPool();
            nextGen.EnsureDistinctPools();
            nextGen.PoolList[0].PossibleKeysList.Sort(ComparatorForSorting);
            nextGen.PoolList[1].PossibleKeysList.Sort(ComparatorForSorting);
            gen = nextGen;
            GenerationNum++;
        }

        public void AutoSimulation()
        {
            SimulationStart();

            do
            {
                int nextGens = (int)(0.3 * GenerationNum);
                if (nextGens > 120)
                {
                    double currentBestFitness = gen.GetBestKey().FitnessValue;
                    double tempBestFitness = 0;
                    for (int i = 0; i < nextGens; i++)
                    {
                        tempBestFitness = gen.GetBestKey().FitnessValue;
                        SimulationStep();
                    }

                    if (tempBestFitness <= currentBestFitness)
                    {
                        break;
                    }
                }
                SimulationStep();

            } while (true);
        }

        public void GetLastGenInfo()
        {
            Console.WriteLine($"Generation - {GenerationNum}\n" +
                $"Best Key - {gen.GetBestKey().KeyValue}\n" +
                $"Best Fitness - {gen.GetBestKey().FitnessValue}\n" +
                $"Decoded text:\n{gen.GetBestKey().DecodeByKeyValue(EncryptedText)}");
        }
    }
}
