// Helper.cpp : Implementation of CHelper

#include "stdafx.h"
#include "Helper.h"


// CHelper



STDMETHODIMP CHelper::Greeting(BSTR name, BSTR* retval)
{
	CComBSTR temp("Hello ");
    temp += name;
	*retval = temp.Detach();
	return S_OK;
}
