import http.server
import json
import struct


def edit_gltf(gltf_data):
    gltf = json.loads(gltf_data)
    gltf['asset']['generator'] += ', Maxis Python Server'
    gltf['scenes'][0]['name'] = 'serverGLTF'
    gltf['buffers'][0]['uri'] = 'serverGLTF.bin'
    gltf['nodes'][0]['name'] = 'serverGLTF'
    gltf['meshes'][0]['name'] = 'serverGLTF_Mesh'
    GltfInfos.generalLength = gltf['buffers'][0]['byteLength']
    GltfInfos.posBufferLength = gltf['bufferViews'][0]['byteLength']
    GltfInfos.offset = 0
    GltfInfos.stride = gltf['bufferViews'][0]['byteStride']
    # switch minX and maxX in gltf file because when you negate all xVals this has to be done
    minX = gltf['accessors'][0]['min'][0]
    maxX = gltf['accessors'][0]['max'][0]
    gltf['accessors'][0]['min'][0] = -maxX
    gltf['accessors'][0]['max'][0] = -minX
    # change color
    gltf['materials'][0]['pbrMetallicRoughness']['baseColorFactor'] = [0.0, 0.7, 0.5, 0.35]
    gltf['materials'][0]['emissiveFactor'] = [0.0, 0.7, 0.5]
    GltfInfos.receivedGLTF = True
    modified_gltf_data = json.dumps(gltf).encode()
    return modified_gltf_data


def edit_bin(bin_data):
    # to_string()
    # basierend auf dem vorher gesendeten gltffile wird die .bin file bearbeitet.
    # gibt es keine gltf infos, wird die datei unverändert zurück gegeben.
    if GltfInfos.receivedGLTF:
        # print('negating all xPositions!')
        # leeres byte array
        modified_data = b''
        # alle bytes bis zum offset einfügen (entspricht dem ersten buffer, die positionen sind im 2.)
        modified_data += bin_data[:GltfInfos.offset]

        for i in range(GltfInfos.offset, GltfInfos.posBufferLength, GltfInfos.stride):
            # https://docs.python.org/3/library/stdtypes.html?highlight=list#sequence-types-list-tuple-range
            chunk = bin_data[i:i + 4]  # 12Byte=pX, pY, pZ -> ich hole mir immer die ersten 4 bytes die pX entsprechen
            # https://docs.python.org/3/library/struct.html#format-characters

            if len(chunk) == 4:
                value = struct.unpack('f', chunk)[0]  # bytes zu float
                negated_value = -value
                negated_bytes = struct.pack('f', negated_value)  # float zu bytes
                modified_data += negated_bytes + bin_data[i + 4:i + GltfInfos.stride]
            else:
                # print(len(chunk))
                modified_data += chunk + bin_data[i + len(chunk):i + GltfInfos.stride]
            # debugging:
            # print(chunk.hex())
            # print('Value: ' + str(value) + ", Negated Value: " + str(negated_value))
            # print(negated_bytes.hex())
            # print('#############')

            # verarbeitete 4 Bytes speichern und die nächsten x bytes anhängen

        modified_data += bin_data[GltfInfos.posBufferLength:]
        return modified_data
    else:
        return bin_data


# adapted from https://parsiya.net/blog/2020-11-15-customizing-pythons-simplehttpserver/ &
# https://docs.python.org/3/library/http.server.html
class MyHTTPRequestHandler(http.server.SimpleHTTPRequestHandler):
    # erbt von klasse in den Klammern um dann PostMethode zu überschreiben

    # Überschreibe die do_GET() Methode
    def do_GET(self):
        if self.path == '/downloadGLTF':
            self.send_response(200)
            self.send_header('Content-type', 'application/json')
            self.end_headers()
            with open('GLTF/ExportedGLTF.gltf', 'rb') as f:
                self.wfile.write(f.read())
        elif self.path == '/downloadBIN':
            self.send_response(200)
            self.send_header('Content-type', 'application/json')
            self.end_headers()
            with open('GLTF/ExportedGLTF.bin', 'rb') as f:
                self.wfile.write(f.read())
        else:
            self.send_response(200)
            self.end_headers()
            self.wfile.write(bytes("Server rennt!", "utf-8"))

    # Überschreibe die do_POST() Methode
    def do_POST(self):
        if self.path == '/uploadGLTF':
            # gltf datei ein wenig verändern, damit man sieht dass damit gearbeitet werden kann
            content_length = int(self.headers['Content-Length'])
            # Lese die übertragenen Daten
            uploaded_gltf = self.rfile.read(content_length)
            with open('GLTF/ExportedGLTF.gltf', 'wb') as f:
                f.write(uploaded_gltf)
            modified_gltf = edit_gltf(uploaded_gltf)
            self.send_response(200)
            self.send_header('Content-Type', 'application/gltf+json')
            self.send_header('Content-length', str(len(modified_gltf)))
            self.end_headers()
            self.wfile.write(modified_gltf)
        elif self.path == '/uploadBIN':  # wird an yz-Ebene gespiegelt und zurückgegeben
            content_length = int(self.headers['Content-Length'])
            uploaded_bin = self.rfile.read(content_length)
            with open('GLTF/ExportedGLTF.bin', 'wb') as f:
                f.write(uploaded_bin)
            modified_bin = edit_bin(uploaded_bin)
            self.send_response(200)
            self.send_header('Content-Type', 'application/octet-stream')
            self.send_header('Content-length', str(len(modified_bin)))
            self.end_headers()
            self.wfile.write(modified_bin)


# klasse zum zwischenspeichern einiger relevanter gltfFile infos für .bin Verarbeitung
class GltfInfos:
    receivedGLTF = False
    offset = -1
    stride = -1
    generalLength = -1
    posBufferLength = -1


def to_string():
    print(str(GltfInfos.offset) + ", " + str(GltfInfos.stride) + ", " + str(GltfInfos.generalLength) + ", " + str(
        GltfInfos.posBufferLength))


# code von https://docs.python.org/3/library/http.server.html
def run(server_class=http.server.HTTPServer, handler_class=MyHTTPRequestHandler):
    server_address = ('', 8000)  # localhost-server auf port 8000
    httpd = server_class(server_address, handler_class)
    httpd.serve_forever()


run()
