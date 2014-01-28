// dllmain.h : Declaration of module class.

class CLegacyCOMModule : public ATL::CAtlDllModuleT< CLegacyCOMModule >
{
public :
	DECLARE_LIBID(LIBID_LegacyCOMLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_LEGACYCOM, "{0AB61771-137A-4065-BD6C-AD3C47114011}")
};

extern class CLegacyCOMModule _AtlModule;
