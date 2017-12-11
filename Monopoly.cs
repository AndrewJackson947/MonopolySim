using System.Diagnostics;
using System;
using System.IO;



public class Boardspace
{
	public string SpaceName;
	public string SpaceType;

	public Boardspace Next;
	public Boardspace Previous;
	
	public Boardspace(){}


	//Constructor for String only input
	public Boardspace(string Name, string Type)
	{		
		//Assign name of boardspace
		this.SpaceName = Name;
		this.SpaceType = Type;
	}

	//Constructor for Boardspace input
	public Boardspace(ref Boardspace BoardspaceToCopy)
		{PopulateBoardspace(ref BoardspaceToCopy.Next, ref BoardspaceToCopy.Previous, BoardspaceToCopy.SpaceName, BoardspaceToCopy.SpaceType);}

	//Constructor for Property Input
	public Boardspace(ref Property BoardspaceToCopy)
		{PopulateBoardspace(ref BoardspaceToCopy.Next, ref BoardspaceToCopy.Previous, BoardspaceToCopy.SpaceName, BoardspaceToCopy.SpaceType);}

	//Populate boardspace data from constructor
	public void PopulateBoardspace(ref Boardspace NextIn, ref Boardspace PreviousIn, string Name, string SpaceTypeIn)
	{
		//Assign correct previous and next values to current boardspace		
		this.Next = NextIn;
		this.Previous = PreviousIn;

		//Reassign previous/next values for adjacent boardspaces
		this.Next.Previous = this;
		this.Previous.Next = this;		

		//Assign name of boardspace
		this.SpaceName = Name;
		this.SpaceType = SpaceTypeIn;
	}

}

//May want to derive from property instead
public class IncomeTaxSpace : Boardspace
{
	public Player Owner; //Always Banker
	
	
	public IncomeTaxSpace(Boardspace BoardspaceIn, Player OwnerIn) : base(ref BoardspaceIn)
	{
		Owner = OwnerIn;
	}
}

//May want to derive from property instead
public class LuxuryTax : Boardspace
{
	public Player Owner; //Always Banker
	
	

	public LuxuryTax(Boardspace BoardspaceIn, Player OwnerIn) : base(ref BoardspaceIn)
	{
		Owner = OwnerIn;
	}
}

public class Jail : Boardspace
{
	
	public Jail(Boardspace BoardspaceIn) : base(ref BoardspaceIn){}
}

public class GoToJail : Boardspace
{

	public GoToJail(Boardspace BoardspaceIn) : base(ref BoardspaceIn){}

}

public class CardSpace : Boardspace
{
	public CardSpace(Boardspace BoardspaceIn) : base(ref BoardspaceIn){}

}

public class GoSpace : Boardspace
{
	public GoSpace(ref Boardspace BoardspaceIn) : base(ref BoardspaceIn){}
}

public class ChanceCardSpace : CardSpace
{
	public ChanceCardSpace(Boardspace BoardspaceIn) : base(BoardspaceIn){}
}

public class CommunityChestCardSpace : CardSpace
{
	public CommunityChestCardSpace(Boardspace BoardspaceIn) : base(BoardspaceIn){}
}

public class Property : Boardspace
{	
	public long CostToBuy;
	public Player Owner;

	public Property(){}	

	public Property(long CostToBuyIn, ref Boardspace PropertySpaceIn, Player InitialOwner) : base(ref PropertySpaceIn)
	{
		this.CostToBuy = CostToBuyIn;
		this.Owner = InitialOwner;
	}

	public Property(ref Property PropertyToClone) : base(ref PropertyToClone)
	{
		this.CostToBuy = PropertyToClone.CostToBuy;
		this.Owner = PropertyToClone.Owner;
		
	}

}
public class TitleProperty : Property
{



	public long Rent;
	long RentColorset;
	long RentOneHouse;
	long RentTwoHouse;
	long RentThreeHouse;
	long RentFourHouse;
	public long RentHotel;

	long HouseCost;
	long HotelCost;
	
	long MortgageVal;
	long MortgagePay;

	public TitleProperty(string NameIn, long RentIn, long RentColorsetIn, long RentOneHouseIn, long RentTwoHouseIn, 
			long RentThreeHouseIn, long RentFourHouseIn, long RentHotelIn, long HouseCostIn, 
			long HotelCostIn, long MortgageValIn, long MortgagePayIn, ref Property BaseProperty) : base(ref BaseProperty)
	{	
		this.SpaceName = NameIn;
		this.Rent = RentIn;
		this.RentColorset = RentColorsetIn;
		this.RentOneHouse = RentOneHouseIn;
		this.RentTwoHouse = RentTwoHouseIn;
		this.RentThreeHouse = RentThreeHouseIn;
		this.RentFourHouse = RentFourHouseIn;
		this.RentHotel = RentHotelIn;
		this.HouseCost = HouseCostIn;
		this.HotelCost = HotelCostIn;
		this.MortgageVal = MortgageValIn;
		this.MortgagePay = MortgagePayIn;
		
		Console.WriteLine("Title: " + this.GetType());
	}


}

public class Utility : Property
{
	static long TotalUtilities;
	
	public Utility(Property BaseProperty) : base(ref BaseProperty)
	{	
		

		TotalUtilities = TotalUtilities + 1;		
	}

	public void PayUtility(Player PlayerLanded)
	{
	
	}
}	

public class Railroad : Property
{
	static long TotalRoads;	
	
	public Railroad(Property BaseProperty) : base(ref BaseProperty)
	{	


		TotalRoads = TotalRoads + 1;
		
	}	
	
	public void TakeRide(Player PlayerLanded){}
}

public class Player
{
	Player NextPlayer;
	Player PrevPlayer;

	long CurrRoll;
	long BankBalance;
	
	string PlayerPiece;
	string PlayerName;
	
	public Player(string PlayerPieceIn, string PlayerName)
	{	
	
		BankBalance = 1500;
		PlayerPiece = PlayerPieceIn;
		PlayerName = PlayerName;
	}


	public long ReturnBalance()
	{
		return BankBalance;
	}

	public long DoubleDieRoll()
	{
		Random rnd = new Random(); 

		long Die1 = rnd.Next(1,6);
		long Die2 = rnd.Next(1,6);
		
		CurrRoll = Die1 + Die2;
		
	} 

}

public class Board
{
	public Boardspace GoSpace;
	public static string FP = System.IO.Directory.GetCurrentDirectory();
	public void Board()
	{
			
				
		TitleProperty TempTitleProperty;
		Boardspace temp;
		Boardspace nexttemp = null;
		CurrBoard = new Board();	
		
		//Set up double circular linked list of board spaces
		string BoardSpacesTblFP = System.String.Concat(this.FP,"/Raw Data/BoardSpacesTbl.csv");
		var BoardSpacesTbl = new StreamReader(@BoardSpacesTblFP);
		
		var line = BoardSpacesTbl.ReadLine();
		Boardspace prevtemp = new Boardspace();
		//Set up linked list of board spaces
		while (!BoardSpacesTbl.EndOfStream)
		{
			line = BoardSpacesTbl.ReadLine();
			var values = line.Split(',');
			
			
			temp = new Boardspace(values[0], values[1]);
			
			if (values[1] == "GO") 
			{
				this.CurrBoard.GoSpace = temp;
			}
			else
			{
				temp.Previous = prevtemp;
				prevtemp.Next = temp;
			}
			

			prevtemp = temp;	
			//Console.WriteLine(values[0]);		
		    	
		}

		
		this.CurrBoard.GoSpace.Previous = prevtemp;
		prevtemp.Next = this.CurrBoard.GoSpace;


		BoardSpacesTbl = new StreamReader(@BoardSpacesTblFP);
		
		line = BoardSpacesTbl.ReadLine();
		
		
		
		temp = this.CurrBoard.GoSpace; 
		
		do 
		{
			if (temp.SpaceType == "GO") {this.CurrBoard.GoSpace = new GoSpace(ref temp);}
			if (temp.SpaceType == "Community Chest") {new CommunityChestCardSpace(temp);}
			if (temp.SpaceType == "Chance") {new ChanceCardSpace(temp);}
			if (temp.SpaceType == "Jail") {new Jail(temp);}
			if (temp.SpaceType == "Income Tax") {new IncomeTaxSpace(temp, this.Banker);}
			if (temp.SpaceType == "Luxury Tax") {new LuxuryTax(temp, this.Banker);}
			if (temp.SpaceType == "GO TO JAIL") {new GoToJail(temp);}
			temp = temp.Next;
		}
		while (temp.SpaceName != CurrBoard.GoSpace.SpaceName);





		string PropertyTblFP = System.String.Concat(this.FP,"/Raw Data/PropertyTbl.csv");		
		var PropertyTbl = new StreamReader(@PropertyTblFP);

		Property TempProperty;
		temp = this.CurrBoard.GoSpace;
		line = PropertyTbl.ReadLine();


		
		//Set up property
		while (!PropertyTbl.EndOfStream)
		{
			line = PropertyTbl.ReadLine();			
			//Console.WriteLine(line);
			var values = line.Split(',');

			
			string PropType = values[3];
			temp = FindSpaceName(values[1]);
			TempProperty = new Property(Convert.ToInt64(values[2]), ref temp, this.Banker);
			
			//Find out if property is TitleProperty,Railroad or utility and create appropriate object			
			if (PropType == "Title Property")
			{	
				
				var values2 = ReturnPropertyData(values[1]);
				Console.WriteLine(values2[1]);
				TempTitleProperty = new TitleProperty(values2[1], Convert.ToInt64(values2[2]), Convert.ToInt64(values2[3]), Convert.ToInt64(values2[4]), 
							Convert.ToInt64(values2[5]), Convert.ToInt64(values2[6]), Convert.ToInt64(values2[7]), Convert.ToInt64(values2[8]),
							 Convert.ToInt64(values2[9]), Convert.ToInt64(values2[10]), Convert.ToInt64(values2[11]), Convert.ToInt64(values2[12]), ref TempProperty);
				Console.WriteLine(this.CurrBoard.GoSpace.Next.GetType());
				Console.WriteLine(TempTitleProperty.GetType());				
							
			}
			if (PropType == "Railroad")
			{
				
				Railroad TempRail;
				TempRail = new Railroad(TempProperty);
			}
			if (PropType == "Utility")
			{
				Utility TempUtil;
				TempUtil = new Utility(TempProperty); 
				
			}
			
			
			//Console.WriteLine(TempProperty.SpaceName);	
					
		}	
	}

	public string[] ReturnPropertyData(string Crit)
	{
		string TitlePropertyTblFP = System.String.Concat(this.FP,"/Raw Data/TitlePropertyTbl.csv"); 
		var TitlePropertyTbl = new StreamReader(@TitlePropertyTblFP);
		string line = TitlePropertyTbl.ReadLine();
		

		while (!TitlePropertyTbl.EndOfStream)
		{
			line = TitlePropertyTbl.ReadLine();				
			var values2 = line.Split(',');

			if(values2[1] == Crit){
				//Console.WriteLine("MatchFound!");				
				return values2;}
		}
		Console.WriteLine("NoMatchFound!");	
		string[] BlankAry = new string[] {"",""};
		return BlankAry;
	}
	


	public Boardspace FindSpaceName(string Crit)
	{
		
		Boardspace temp;
		temp = CurrBoard.GoSpace; 
		
		do 
		{
			//Console.WriteLine(temp.SpaceName);
			if (temp.SpaceName == Crit){return temp;}
			temp = temp.Next;
		}
		while (temp.SpaceName != CurrBoard.GoSpace.SpaceName);
		
		Console.WriteLine("NO MATCH FOUND!");
		return new Boardspace();
	}
}


public class Monopoly
{
	public Board CurrBoard = new Board();//Initialize board.
	public Player Banker = new Player(1000000, "Banker", "Banker"); //Initialize Banker.
	public Player FirstPlayer;
	public long PlayerCount;
	public static string FP = System.IO.Directory.GetCurrentDirectory(); //Path to CSV files. May want to make this apart of a separate class
	public Queue PlayersQueue = new Queue();

	public Monopoly()
	{
		
		
	} 

	//Need to add a way to restrict player pieces to ones that have not been used yet
	public void NewPlayer(string PlayerName, string PlayerPiece)
	{	
		
		if (this.PlayerCount == 0)
		{
			this.FirstPlayer = new Player(PlayerPiece, PlayerName);
			this.FirstPlayer.NextPlayer = this.FirstPlayer;
 			this.FirstPlayer.PrevPlayer = this.FirstPlayer;
		}
		else
		{
			Player tempPlayer = this.FirstPlayer;
			Player TempPlayerToAdd;
			Player TempPlayerAfterAdd;
		
			long k = 1;
			while (k < this.PlayerCount)
			{
				tempPlayer = tempPlayer.NextPlayer;
				k = k + 1;
			}
			TempPlayerToAdd = new Player(PlayerPiece, PlayerName);
			TempPlayerAfterAdd = tempPlayer.NextPlayer 

			TempPlayerToAdd.NextPlayer = TempPlayerAfterAdd;
			TempPlayerToAdd.PrevPlayer = tempPlayer;
					
			TempPlayerAfterAdd.PrevPlayer = TempPlayerToAdd;
			tempPlayer.NextPlayer = TempPlayerToAdd; 

			
		}
		
		this.PlayerCount = this.PlayerCount + 1;

	}

	public void StartGame()
	{
		InitRoll()
	}

	public void InitRoll()
	{
		Player tempPlayer = this.FirstPlayer;
		boolean inOrder = false;
		Player LargestInIter;
		long LargestRollInIter = 0;
		Player TempStart; 

		//Everyone Rolls					
		while (tempPlayer.NextPlayer.PlayerPiece !=  this.FirstPlayer.PlayerPiece)
		{
			tempPlayer.DoubleDieRoll();
		}

		
		tempPlayer = this.FirstPlayer;
		while (this.FirstPlayer != null)
		{
			while (tempPlayer.NextPlayer.PlayerPiece !=  this.FirstPlayer.PlayerPiece)
			{
				if (LargestRollInIter < tempPlayer.CurrRoll)
				{
					LargestInIter = tempPlayer
					LargestRollInIter = tempPlayer.CurrRoll;
				}

				tempPlayer = tempPlayer.NextPlayer;
			}
			
			//If largest roll was  
			if (LargestInIter.PlayerPiece ==  this.FirstPlayer.PlayerPiece)
			{	

				BeginSortedNodes = this.FirstPlayer;
				BeginSortedNodes = this.FirstPlayer.NextPlayer;
			}
			if (tempPlayer.NextPlater.PlayerPiece ==  this.FirstPlayer.PlayerPiece)
			{
							
			}
			if (tempPlayer.NextPlater.PlayerPiece !=  this.FirstPlayer.PlayerPiece && tempPlayer.PlayerPiece != this.FirstPlayer.PlayerPiece)
			{
							
			}			
		}			
	}

	public void SetUpCards()
	{
		string PlayerPiecesTblFP = System.String.Concat(Monopoly.FP,"/Raw Data/PlayerPiecesTbl.csv");
		var PlayerPiecesTbl = new StreamReader(@PlayerPiecesTblFP);
		
		string CommunityChestCardTblFP = System.String.Concat(Monopoly.FP,"/Raw Data/CommunityChestCardTbl.csv");		
		var CommunityChestCardTbl = new StreamReader(@CommunityChestCardTblFP);

		string ChanceCardTblFP = System.String.Concat(Monopoly.FP,"/Raw Data/ChanceCardTbl.csv");
		var ChanceCardTbl = new StreamReader(@ChanceCardTblFP);	
	
	}


}



public class Test
{
	public static void Main(string[] args)
	{
        	//Declare Monopoly instance
		string CurrentFP;			

		Monopoly CurrGame = new Monopoly();
		CurrGame.NewPlayer("Dog","Andrew")
		CurrGame.NewPlayer("Ship","Fei")

		//Test
		Boardspace temp;
		temp = CurrGame.CurrBoard.GoSpace; 
		Console.WriteLine("BEGIN TEST");
		do 
		{
			Console.WriteLine(temp.SpaceName);
			Console.WriteLine(temp.SpaceType);	
			Console.WriteLine(temp.GetType());
			temp = temp.Next;			
			if (temp is TitleProperty) 
			{
				Console.WriteLine(temp is TitleProperty);
				TitleProperty tempTitle = (TitleProperty)temp;
				Console.WriteLine(tempTitle.Rent);			
			} 
		}
		while (temp.SpaceName != CurrGame.CurrBoard.GoSpace.SpaceName);
		
		
	}
}