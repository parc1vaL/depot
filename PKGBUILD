pkgname=depot
pkgver=1.1.0
pkgrel=1
pkgdesc="Depot tracking tool"
arch=('x86_64')
url='https://github.com/parc1vaL/depot'
license=("APACHE")
depends=("icu")
makedepends=("dotnet-sdk")
options=("staticlibs" "!strip")
source=("git+$url")
sha256sums=('SKIP')

build() {
  cd "$pkgname"
  MSBUILDDISABLENODEREUSE=1 dotnet publish \
    --configuration Release \
    --self-contained true \
    --runtime linux-x64 \
    -p:PublishTrimmed=true \
    -p:DebugType=None \
    -p:DebugSymbols=false \
    --output ./out \
    ./depot.csproj
}

package() {
  cd "$pkgname"

  install -d $pkgdir/usr/{bin,lib}
  cp -r ./out/ "$pkgdir/usr/lib/$pkgname"
  ln -s "/usr/lib/$pkgname/$pkgname" "$pkgdir/usr/bin/$pkgname"
}

