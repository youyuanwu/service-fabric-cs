cmake_minimum_required(VERSION 3.23)

project(Example VERSION 0.1.0 LANGUAGES CSharp)

message(STATUS "fetching fabric_metadata")
include(FetchContent)
FetchContent_Declare(fabric_metadata
    GIT_REPOSITORY https://github.com/youyuanwu/fabric-metadata.git
    GIT_TAG a1bcca6ad6f6a772c9e5ff4bdba80ae5e5f24cfc
)
FetchContent_GetProperties(fabric_metadata)
if(NOT fabric_metadata_POPULATED)
    FetchContent_Populate(fabric_metadata)
    # add all targets to main build. should not be too many for now.
    # add_subdirectory(${fabric_metadata_SOURCE_DIR} ${fabric_metadata_BINARY_DIR} EXCLUDE_FROM_ALL)
endif()

#add_subdirectory(src)