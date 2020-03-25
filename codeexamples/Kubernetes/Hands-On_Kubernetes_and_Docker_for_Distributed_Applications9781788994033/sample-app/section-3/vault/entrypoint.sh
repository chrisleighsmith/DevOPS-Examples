	#! /bin/bash
	
	export FOO=$(curl \
	    --header "X-Vault-Token: $VAULT_TOKEN" \
	    http://vault:8200/v1/secret/my-secret \
	    | jq '.data.foo')
	
	export ZIP=$(curl \
	    --header "X-Vault-Token: $VAULT_TOKEN" \
	    http://vault:8200/v1/secret/my-secret \
    | jq '.data.zip')

    python main.py
