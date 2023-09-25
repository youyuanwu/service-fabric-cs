cmake_minimum_required(VERSION 3.23)

project(service_fabric_cs VERSION 0.1.0 LANGUAGES CSharp)

set(service_fabric_cs_MAIN_PROJECT OFF)
if (CMAKE_CURRENT_SOURCE_DIR STREQUAL CMAKE_SOURCE_DIR)
    set(service_fabric_cs_MAIN_PROJECT ON)
endif()

message(STATUS "fetching fabric_metadata")
include(FetchContent)
FetchContent_Declare(fabric_metadata
    GIT_REPOSITORY https://github.com/youyuanwu/fabric-metadata.git
    GIT_TAG 28f81014bf3a2f4de7718242c2d091df96a76752
)
FetchContent_GetProperties(fabric_metadata)
if(NOT fabric_metadata_POPULATED)
    FetchContent_Populate(fabric_metadata)
    # add all targets to main build. should not be too many for now.
    # add_subdirectory(${fabric_metadata_SOURCE_DIR} ${fabric_metadata_BINARY_DIR} EXCLUDE_FROM_ALL)
endif()

#add_subdirectory(src)
find_program(dotnet_exe
    NAMES dotnet.exe
    REQUIRED
)

add_custom_target(dotnet ALL
    COMMAND ${dotnet_exe} build ${CMAKE_CURRENT_SOURCE_DIR}/dirs.proj
)

if(service_fabric_cs_MAIN_PROJECT)
# code generation
find_program(tlbimp_exe
    NAMES tlbimp.exe
    REQUIRED
)

set(_gen_dir ${CMAKE_CURRENT_SOURCE_DIR}/src/gen)
set(_tlb_src_dir ${fabric_metadata_SOURCE_DIR}/tlb)

# add_custom_command(
#     OUTPUT ${_out_header_cpp}
#     COMMAND powershell.exe -file "${fabric-metadata_SOURCE_DIR}/scripts/gen_cpp.ps1" -Source ${_header_c} -OutFile ${_out_header_cpp}
#     WORKING_DIRECTORY ${CMAKE_SOURCE_DIR} # coan relative to the cmake source dir
#     DEPENDS ${_header_c}
# )

add_custom_target(generate_cs
    # types
    COMMAND ${tlbimp_exe} ${_tlb_src_dir}/FabricTypes.tlb /silence:3016 /out:${_gen_dir}/FabricTypesCs.dll
    # client
    COMMAND ${tlbimp_exe} ${_tlb_src_dir}/FabricClient.tlb /silence:3016 /silence:3015 
        /out:${_gen_dir}/FabricClientCs.dll /strictref /reference:${_gen_dir}/FabricTypesCs.dll
        /tlbreference:${_tlb_src_dir}/FabricTypes.tlb
  WORKING_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
)

endif(service_fabric_cs_MAIN_PROJECT)