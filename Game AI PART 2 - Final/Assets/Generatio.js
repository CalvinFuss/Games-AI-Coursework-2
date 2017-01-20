#pragma strict


var myTerrain : Terrain;
var myTerrainData : TerrainData;
 
function Start() 
{
    if ( !myTerrain )
    {
        myTerrain = Terrain.activeTerrain; // find the active terrain
    }
     
    myTerrainData = myTerrain.terrainData; // store the terrainData
     
    ReadTerrainAndPlaceCube();
}
 
function ReadTerrainAndPlaceCube() 
{
    var heights : float[,] = myTerrainData.GetHeights( 0, 0, myTerrainData.heightmapWidth, myTerrainData.heightmapHeight );
     
    for ( var y = 0; y < myTerrainData.heightmapHeight; y ++ ) 
    {
        for ( var x = 0; x < myTerrainData.heightmapWidth; x ++ ) 
        {
            if ( x % 5 == 0 && y % 5 == 0 ) // place a cube every 20*20 units
            {
                var actualHeight : float = heights[y,x] * myTerrainData.size.y; // need to multiply the height by the size.y of the terrain
                 
                var cube : GameObject = GameObject.CreatePrimitive( PrimitiveType.Cube ); // create a cube
                 
                cube.transform.position = new Vector3( x, actualHeight, y ); // position it at the actual height
                
            }
        }
    }

    
}