static var distance3 : float = 5;

function Update () {

	var hit: RaycastHit;
	if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), hit)){

	//print("inside Raycast3, distance3 is:");
	print(distance3);

	distance3 = hit.distance;

	}
}