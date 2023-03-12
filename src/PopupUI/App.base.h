#pragma once
#include <winrt/base.h>
#include "XamlMetaDataProvider.h"
#include <winrt/Windows.UI.Xaml.Interop.h>
#include <winrt/Windows.UI.Xaml.Markup.h>

namespace winrt::PopupUI::implementation
{
    template <typename D>
    struct App_baseWithProvider : public App_base<D, ::winrt::Windows::UI::Xaml::Markup::IXamlMetadataProvider>
    {
        using IXamlType = ::winrt::Windows::UI::Xaml::Markup::IXamlType;
        IXamlType GetXamlType(::winrt::Windows::UI::Xaml::Interop::TypeName const& type)
        {
            return AppProvider()->GetXamlType(type);
        }
        IXamlType GetXamlType(::winrt::hstring const& fullName)
        {
            return AppProvider()->GetXamlType(fullName);
        }
        ::winrt::com_array<::winrt::Windows::UI::Xaml::Markup::XmlnsDefinition> GetXmlnsDefinitions()
        {
            return AppProvider()->GetXmlnsDefinitions();
        }
    private:
        bool _contentLoaded{ false };
        com_ptr<XamlMetaDataProvider> _appProvider;
        com_ptr<XamlMetaDataProvider> AppProvider()
        {
            if (!_appProvider)
            {
                _appProvider = make_self<XamlMetaDataProvider>();
            }
            return _appProvider;
        }
    };

    template <typename D>
    using AppT2 = App_baseWithProvider<D>;
}