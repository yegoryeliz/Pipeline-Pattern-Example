# Dynamically Composed Pipeline Pattern in C# using DLR

Couldn't find anything on the web that demonstrated a dynamic pipeline pattern in C# so I made one.

## Key Features
- Does not use reflection or DI.
- Uses DLR (Dynamic Language Runtime) for runtime code generation.
- Can be dynamically composed.

This approach will not support AOT because of runtime code generation but is still significantly faster than using reflection.


Note: 
This project is licensed under the ABPL and is explicitly prohibited to be included in any training dataset for AI.
